var socketId;
var uuid = '1105';
var message_table = {};
var END_OF_MESSAGE = ":END_OF_MESSAGE:";

function ab2str(buf) {
  return String.fromCharCode.apply(null, new Uint8Array(buf));
  //return decodeURIComponent(escape(String.fromCharCode.apply(null, buf)));
}

function str2ab(str) {
    var buf = new ArrayBuffer(str.length); // 2 bytes for each char
    var bufView = new Uint8Array(buf);
    for (var i=0, strLen=str.length; i<strLen; i++) {
		bufView[i] = str.charCodeAt(i);
    }
    return buf;
  
    // var strUtf8 = unescape(encodeURIComponent(str));
	// var ab = new Uint8Array(strUtf8.length);
    // for (var i = 0; i < strUtf8.length; i++) {
        // ab[i] = strUtf8.charCodeAt(i);
    // }
    // return ab;
}

//chrome.app.runtime.onLaunched.addListener(function() {
	chrome.bluetooth.getAdapterState(function(adapter) {
		console.log("Adapter " + adapter.address + ": " + adapter.name);
	});

	chrome.bluetoothSocket.create(function(createInfo) {
		socketId = createInfo.socketId;
		chrome.bluetoothSocket.listenUsingRfcomm(socketId, uuid, function() {
			console.log("Socket created: " + socketId);
		});
	});

	chrome.bluetoothSocket.onAccept.addListener(function(acceptInfo) {
		console.log("Got a client " + JSON.stringify(acceptInfo));
		if( acceptInfo.socketId != socketId )
			return;
  
		message_table[acceptInfo.clientSocketId] = "";
		
		//console.log("Sending hello");
		//chrome.bluetoothSocket.send(acceptInfo.clientSocketId, str2ab("From chrome\n\n"), function(){});

		chrome.bluetoothSocket.setPaused(acceptInfo.clientSocketId, false);
  
	});

    // Accepted sockets are initially paused,
	// set the onReceive listener first.
	chrome.bluetoothSocket.onReceive.addListener(function(receiveInfo) {
		//console.log("Received " + JSON.stringify(receiveInfo));
		var json = ab2str(receiveInfo.data);
		//console.log("Message: " + json);
		
		message_table[receiveInfo.socketId] += json;
		
		var full_message = message_table[receiveInfo.socketId];
		if( full_message.match(END_OF_MESSAGE) ) {
			//console.log("Before " + full_message);
			full_message = full_message.replace(END_OF_MESSAGE, "");
			//console.log("After " + full_message);
			var obj = JSON.parse(full_message);
			notifyMe(obj);
			//console.log("Closing client " + receiveInfo.socketId);
			chrome.bluetoothSocket.close(receiveInfo.socketId);
			delete message_table[receiveInfo.socketId];
			console.log(Object.keys(message_table));
		}
	});

	function notifyMe(obj) {
		if (Notification.permission !== "granted")
			Notification.requestPermission();

		
		var date = new Date()
		var notificationId = " at " + date.toTimeString() + " " + date.getMilliseconds()
		chrome.notifications.create(notificationId, {
			iconUrl: decodeImage(obj.icon),
			type: "basic",
			message: obj.text || "unknown message",
			title: obj.title || "unknown title"
		}, function() {
			setTimeout(function(){
				chrome.notifications.clear(notificationId, function() {})
			}, 12000);  
		});
	}
//});

function decodeImage(base64) {
	if(base64 != "null" ) {
		return "data:image/png;base64," + base64;
	}
	return 'logo128.png';
}

function keep_alive() {
	setTimeout(keep_alive, 5000);
}

keep_alive();