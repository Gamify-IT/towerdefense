mergeInto(LibraryManager.library, {
    GetConfiguration: function() {
  	var locationArray = window.location.toString().split("/");
  	var returnStr = locationArray[locationArray.length - 1];
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
});