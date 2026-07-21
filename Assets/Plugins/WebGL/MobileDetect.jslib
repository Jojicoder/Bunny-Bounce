mergeInto(LibraryManager.library, {
  IsMobileBrowser: function () {
    var ua = navigator.userAgent || navigator.vendor || "";
    var isMobileUA = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(ua);
    var isIPadOSDesktopUA = /Macintosh/i.test(ua) && navigator.maxTouchPoints > 1;
    return (isMobileUA || isIPadOSDesktopUA) ? 1 : 0;
  }
});
