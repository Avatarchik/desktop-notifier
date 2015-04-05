# desktop-notifier

Android app for pushing notifications to a desktop. The communication happens over bluetooth serial protocol. On the desktop, a native windows app and a chrome plugin are supported. Events sent from the phone are displayed in the desktop as a popup notification.

Current status - alpha

#### Done

- Implement bluetooth device selection dialog instead of entering desktop's MAC address
- Implement a blacklist to skip pushing unwanted notifications (like keyboard selection etc.)
- Create a user installable apk for android app

#### Todo

- Send more data - like contact picture when receiving a phone call, email subject line text etc.
- Figure out a way to display images in the windows app notification
- Create a user installable chrome plugin
- Create a user installable desktop app
