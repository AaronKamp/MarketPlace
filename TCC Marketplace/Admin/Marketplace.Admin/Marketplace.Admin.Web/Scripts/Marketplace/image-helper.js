var scaleSize = function (maxW, maxH, currW, currH) {
    var ratio = currH / currW;
    if (currW >= maxW && ratio <= 1) {
        currW = maxW;
        currH = currW * ratio;
    } else if (currH >= maxH) {
        currH = maxH;
        currW = currH / ratio;
    }
    return [currW, currH];
}