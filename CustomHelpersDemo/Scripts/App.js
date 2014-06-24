function App(){
	var t = this;
	var callbacks = [];
	var callbacksComplete = [];

	this.onReady = function(callback){
		callbacks[callbacks.length] = callback;
	}
	this.onComplete = function (callback) {
	    callbacksComplete[callbacksComplete.length] = callback;
	}


	$(document).ready(function () {
	    console.log("App - onReady : " + callbacks.length);
		for (var i = 0; i < callbacks.length; i++) {
			callbacks[i]();
		};

		console.log("App - onComplete : " + callbacksComplete.length);
		for (var i = 0; i < callbacksComplete.length; i++) {
		    callbacksComplete[i]();
		};
	});
}

App.instance = null;
App.Inst = function () {
    if (App.instance == null)
        App.instance = new App();
    return App.instance;
}