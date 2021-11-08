// scrolling
window.getElementScrollTop = (id) => document.getElementById(id).scrollTop;
window.getElementScrollMax = (id) => document.getElementById(id).scrollHeight - document.getElementById(id).clientHeight;
window.scrollElementTo = (id, scroll) => document.getElementById(id).scrollTop = scroll;
window.scrollElementToBottom = (id) => document.getElementById(id).scrollTop = getElementScrollMax(id);

// audio
window.playAudio = (id) => document.getElementById(id).play();

// clipboard
window.writeClipboard = (text) => navigator.clipboard.writeText(text);

// notifications
window.requestNotificationPermission = () => Notification.requestPermission().then(p => p === "granted" ? true : false);
window.createNotification = (title, body, icon) => new Notification(title, { body: body, icon: icon });

// chat
window.updateGrowingTextArea = (id) =>
{
	var element = document.getElementById(id);
	element.style.height = 'auto';
	element.style.height = (element.scrollHeight) + 'px';
};