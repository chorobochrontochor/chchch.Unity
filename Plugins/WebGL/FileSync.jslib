mergeInto(LibraryManager.library, {
	SyncFiles: function() {
		FS.syncfs(false, function(error) { });
	}
});