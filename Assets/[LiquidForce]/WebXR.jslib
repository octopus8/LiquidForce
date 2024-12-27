mergeInto(LibraryManager.library, {
  WebXROnApplicationReady: function() {
    document.dispatchEvent(new CustomEvent('onApplicationReady', {  }));	  
  }

});