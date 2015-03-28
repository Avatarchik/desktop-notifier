var socketId;
var uuid = '1105';

function ab2str(buf) {
  return String.fromCharCode.apply(null, new Uint8Array(buf));
}
function str2ab(str) {
  var buf = new ArrayBuffer(str.length); // 2 bytes for each char
  var bufView = new Uint8Array(buf);
  for (var i=0, strLen=str.length; i<strLen; i++) {
    bufView[i] = str.charCodeAt(i);
  }
  return buf;
}

chrome.app.runtime.onLaunched.addListener(function() {
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
  
		//console.log("Sending hello");
		//chrome.bluetoothSocket.send(acceptInfo.clientSocketId, str2ab("From chrome\n\n"), function(){});

		chrome.bluetoothSocket.setPaused(acceptInfo.clientSocketId, false);
  
	});

    // Accepted sockets are initially paused,
	// set the onReceive listener first.
	chrome.bluetoothSocket.onReceive.addListener(function(receiveInfo) {
		console.log("Received " + JSON.stringify(receiveInfo));
		var json = ab2str(receiveInfo.data);
		var obj = JSON.parse(json);
		notifyMe(obj.text, obj.title);
		console.log("Closing client " + receiveInfo.socketId);
	  
		chrome.bluetoothSocket.close(receiveInfo.socketId);
	});

	function notifyMe(message, title) {
		if (Notification.permission !== "granted")
			Notification.requestPermission();

		var date = new Date()
		var notificationId = " at " + date.toTimeString() + " " + date.getMilliseconds()
		chrome.notifications.create(notificationId, {
			iconUrl: 'logo128.png',
			type: "basic",
			message: message,
			title: title
		}, function() {
			setTimeout(function(){
				chrome.notifications.clear(notificationId, function() {})
			}, 12000);  
		});
	}
});