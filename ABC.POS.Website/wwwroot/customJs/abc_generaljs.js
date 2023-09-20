abc_general = {

    dollarParenthAdd: function (value) {
        var result = '$(' + Math.abs(value) + ')';
        return result;
    },

    dollarParenthRemove: function (value) {
        var result = value.replace('$', '').replace('(', '').replace(')', '');
        return -Math.abs(result);
    },

    dollarAdd: function (value) {
        var result = '$' + value;
        return result;
    },

    dollarRemove: function (value) {
        var result = value.replace('$', '');
        return result;
    },

    isNullOrEmpty: function (value) {
        return (value == null || value == "" || value == undefined) ? true : false
    },

    parseDecimal: function (value, decimalplace) {
        var result = parseFloat(value).toFixed(decimalplace);
        return result;
    },

    signRemove: function (value, sign) {
        var result = value.replace(sign, '');
        return result;
    },

    parenthRemove: function (value, sign) {
        var result = value.includes(sign) == true ? this.dollarParenthRemove(value) : this.dollarRemove(value);
        return result;
    },

    parenthAdd: function (value) {
        var result = value.includes("-") == true ? this.dollarParenthAdd(value) : this.dollarAdd(value);
        return result;
    },


}