mergeInto(LibraryManager.library, {

  // Called upon the application being ready, this function dispatches the 'onApplicationReady' event, which is
  // received by the JavaScript code. 
  WebXROnApplicationReady: function() {
    document.dispatchEvent(new CustomEvent('onApplicationReady', {  }));	  
  }

});