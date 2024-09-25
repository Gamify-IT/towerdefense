mergeInto(LibraryManager.library, {
    GetOriginUrl: function() {
        var returnStr = location.origin;
        if(returnStr == null){
            console.log("URL was null");
        }
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
});