ABCPOSWebsite = {
    
    init: function () {
    var allItem_array = [];

        $.ajax({
            type: "GET",
            async: false,
            cache: false,
            url: "/Sale/GetAllItem" ,//@Url.Action("GetAllItem", "Sale"),
            dataType: 'json',
            success: function (data) {
                var results = jQuery.parseJSON(JSON.stringify(data));
                console.log(results);
                $.each(results, function (index, value) {
                    allItem_array.push(value);
                });

            },
            error: function (xhr, status, error) {
                var errorMessage = xhr.status + ': ' + xhr.statusText
            },
        });

        return allItem_array;
        console.log(allItem_array);
    },

    AllItem4Model: function () {
        var allItemModel_array = [];

        $.ajax({
            type: "GET",
            async: false,
            cache: false,
            url: "/Purchase/GetItemListformodal",//@Url.Action("GetAllItem", "Sale"),
            dataType: 'json',
            success: function (data) {
                var results = jQuery.parseJSON(JSON.stringify(data));
                $.each(results, function (index, value) {
                    allItemModel_array.push(value);
                });

            },
            error: function (xhr, status, error) {
                var errorMessage = xhr.status + ': ' + xhr.statusText
            },
        });

        return allItemModel_array;
        console.log(allItem_array);
    },


    AutoItems: function () {

        var allAutoItem_array = [];
        $.ajax({
            type: "GET",
            url:"/Items/AutoCompleteSearchItem", //'@Url.Action("AutoCompleteSearchItem", "Items")',
            async: false,
            dataType: 'json',
            success: function (data) {
                var opts = jQuery.parseJSON(JSON.stringify(data));
                $.each(opts, function (i, d) {
                allAutoItem_array.push(d);
                });
            },
            error: function (xhr, status, error) {
                var errorMessage = xhr.status + ': ' + xhr.statusText
            },
        });

        return allAutoItem_array;

    },

    AllSuppliers: function () {

        var allSuppliers_array = [];
        $.ajax({
            url: "/Purchase/GetSupplierJson", //'@Url.Action("GetSupplierJson", "Purchase")',
            async: false,
            contentType: "application/json;",
            dataType: "json",
            success: function (data) {

                var opts = jQuery.parseJSON(JSON.stringify(data));
                $.each(opts, function (i, d) {
                    allSuppliers_array.push(d);
                });
            },
            error: function (error) {
                //alert(error);
            }
        });

        return allSuppliers_array;

    },

    AllOpenPurchaseOrders: function () {

        var allPurchaseOrders_array = [];
        $.ajax({
            url: "/Purchase/GetOpenPurchase", //'@Url.Action("GetSupplierJson", "Purchase")',
            async: false,
            contentType: "application/json;",
            dataType: "json",
            success: function (data) {

                var opts = jQuery.parseJSON(JSON.stringify(data));
                $.each(opts, function (i, d) {
                    allPurchaseOrders_array.push(d);
                });
            },
            error: function (error) {
                //alert(error);
            }
        });

        return allPurchaseOrders_array;

    },

    AllClosePurchaseOrders: function () {

        var allPurchaseOrders_array = [];
        $.ajax({
            url: "/Purchase/GetPostPurchase", //'@Url.Action("GetSupplierJson", "Purchase")',
            async: false,
            contentType: "application/json;",
            dataType: "json",
            success: function (data) {

                var opts = jQuery.parseJSON(JSON.stringify(data));
                $.each(opts, function (i, d) {
                    allPurchaseOrders_array.push(d);
                });
            },
            error: function (error) {
                //alert(error);
            }
        });

        return allPurchaseOrders_array;

    },

    AllSupplierItems: function () {

        var allsuppileritems_array = [];
        $.ajax({
            url: "/Purchase/GetItemsWithVenodrId", //'@Url.Action("GetSupplierJson", "Purchase")',
            async: false,
            contentType: "application/json;",
            dataType: "json",
            success: function (data) {

                var opts = jQuery.parseJSON(JSON.stringify(data));
                $.each(opts, function (i, d) {
                    allsuppileritems_array.push(d);
                });
            },
            error: function (error) {
                //alert(error);
            }
        });

        return allsuppileritems_array;

    },

    AllItemsReturnByID: function () {

        var allreturnitems_array = [];
        $.ajax({
            url: "/Sale/GetAllItemReturnWithId", //'@Url.Action("GetSupplierJson", "Purchase")',
            async: false,
            contentType: "application/json;",
            dataType: "json",
            success: function (data) {

                var opts = jQuery.parseJSON(JSON.stringify(data));
                $.each(opts, function (i, d) {
                    allreturnitems_array.push(d);
                });
            },
            error: function (error) {
                //alert(error);
            }
        });

        return allreturnitems_array;

    },
}