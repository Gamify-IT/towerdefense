mergeInto(LibraryManager.library, {
    CloseMinigame: function() {
	window.parent.postMessage("CLOSE ME")
    },
});