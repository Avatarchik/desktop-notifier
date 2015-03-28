# desktop-notifier

Android app for pushing notifications to a desktop plugin. The communication happens over bluetooth serial protocol. On the desktop, a chrome plugin listens for events sent from the phone and displays events as they are received.

Current status - pre alpha

#### Todo

- Implement bluetooth device selection dialog instead of entering desktop's MAC address
- Implement a blacklist to skip pushing unwanted notifications (like keyboard selection etc.)
- Send more data - like contact picture when receiving a phone call, email subject line text etc.
- Create a user installable apk for android app
- Create a user installable chrome plugin
