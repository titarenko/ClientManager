//
// commonly used functions and methods
String.prototype.endsWith = function (suffix) {
    return this.indexOf(suffix, this.length - suffix.length) !== -1;
};


//
// return true if string contains argument
String.prototype.contains = function(searchString) {
    return this.indexOf(searchString) != -1;
};


//
// common UI setup
$(function () {
    //
    // enable date editors
    $(".date").datepicker();

    //
    // enable navigation menu filtering
    $('#navigation a').each(function () {
        if (this.href.endsWith(document.location.pathname)) {
            $(this).hide(); // hide link to opened page
        }
    });
});