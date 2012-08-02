//
// commonly used functions and methods

//
//return true if string ends with argument
String.prototype.endsWith = function (suffix) {
    return this.indexOf(suffix, this.length - suffix.length) !== -1;
};


//
// return true if string contains argument
String.prototype.contains = function(searchString) {
    return this.indexOf(searchString) != -1;
};

//
//
/// If string is more then maximum length then it will be cutten for maximum length and last 3 symbols will be changed for "..."
/// For example:
/// var s="This is string";
/// var result=s.Cut(7); //result will be equal "This..."
String.prototype.cut = function (maxLength) {
     return this.length > maxLength ? this.substring(0, maxLength - 3) + "..." : this;
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