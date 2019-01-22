var reader;
var progress = document.querySelector('.percent');


$(document).ready(function () {
    $(".preloader").hide();

    $('#stateList').unbind();
    $('#stateList').change(function (e) {
        GetCityByState();
    });

    $('span[name=productStatus]').unbind();
    $('span[name=productStatus]').click(function (e) {
        UpdateProductStatus($(this).attr("id"));
    });

    $('#searchButton').unbind();
    $('#searchButton').click(function (e) {
        SearchDetail($(this).attr("data-view"));
    });

    $('button[name=updatePromoStatusBtn]').unbind();
    $('button[name=updatePromoStatusBtn]').click(function (e) {
        UpdatePromotionRetailerEntrySatus($(this).attr("data-promo-id"), $(this).attr("data-ret-id"), $(this).attr("data-action"));
    });

    $('img[name=deleteNotification]').unbind();
    $('img[name=deleteNotification]').click(function (e) {
        DeleteNotification($(this).attr("data-id"));
    });


    $('span[name=promotionStatus]').unbind();
    $('span[name=promotionStatus]').click(function (e) {
        UpdatePromotionStatus($(this).attr("id"));
    });

    $('span[name=mediaStatus]').unbind();
    $('span[name=mediaStatus]').click(function (e) {
        UpdateMediaCategoryStatus($(this).attr("id"));
    });

    $('span[name=mediaGallaryStatus]').unbind();
    $('span[name=mediaGallaryStatus]').click(function (e) {
        UpdateUploadedMediaStatus($(this).attr("id"));
    });

    $('span[name=bannerStatus]').unbind();
    $('span[name=bannerStatus]').click(function (e) {
        UpdateBannerStatus($(this).attr("id"));
    });

    $('span[name=festivePointStatus]').unbind();
    $('span[name=festivePointStatus]').click(function (e) {
        UpdateFestivePointStatus($(this).attr("id"));
    });

    $('span[name=messageStatus]').unbind();
    $('span[name=messageStatus]').click(function (e) {
        UpdateMessageStatus($(this).attr("id"));
    });

    if (document.getElementById('files') != null) {
        document.getElementById('files').addEventListener('change', handleFileSelect, false);
    }

    $('#btnUpload').unbind();
    $('#btnUpload').click(function () {
        $(".preloader").show();
        $('#grid').empty();
        // Checking whether FormData is available in browser  
        if (window.FormData !== undefined) {

            var fileUpload = $("#FileUpload1").get(0);
            var files = fileUpload.files;

            // Create FormData object  
            var fileData = new FormData();

            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }

            $.ajax({
                url: '/Dashboard/Upload',
                type: "POST",
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                dataType: "json",
            }).done(function (result) {
                if (result.Status == true) {
                    window.location.href = '/Dashboard/GetProductView';
                }
                else {
                    $("#errorDetail").html(result.ResponseValue);
                }
                $("#resultContainer").html("");
                $("#resultContainer").hide(objResult);
                $(".preloader").hide();
            }).fail(function (error) {
                $("#errorDetail").html(error.statusText);
                $(".preloader").hide();
                //$("#errorDialog").dialog();
            });
        } else {
            alert("FormData is not supported.");
        }
    });

    $("input[type=radio]:radio").change(function () {
        var value = $(this).val();
        var ImageId = $(this).attr("data-id");
        UpdateRetailerPrmotionImageStatus(ImageId, value);
    });
});

function GetCityByState() {
    var id = $("#stateList").val();
    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/GetCityListByState',
        type: 'Post',
        datatype: 'Json',
        data: { Id: id }
    }).done(function (result) {
        if (result == null || result == undefined || result == "") {
            $("#error").html(result);
        }
        else {

            $("#cityList").html("");
            $.each(result, function (key, value) {
                $("#cityList").append($("<option></option>").val(value.cityID).html(value.cityName));
            });
        }

        $(".preloader").hide();
    });
}

function SaveRetailerDetail() {
    $("#error").html("");
    var loginDetail = $('#retailerIdForm').serialize();
    $(".preloader").show();
    $.ajax({
        url: 'SaveRetailerDetail',
        type: 'Post',
        datatype: 'Json',
        data: loginDetail
    }).done(function (result) {
        $("#error").html(result);
        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function SearchDetail(viewName) {
    // var detail = $('#retailerSearchForm').serialize();
    $(".preloader").show();
    var id = $("#filterList").val();
    var value = $("#filterValue").val();
    var uri = "";
    if (viewName == "Retailer") {
        var fromDate = $("#fromDate").val();
        var toDate = $("#toDate").val();
        uri = '/Dashboard/GetRegisteredRetailer?filterType=' + id + '&filterValue=' + value + '&fromDate=' + fromDate + '&toDate=' + toDate;
    }
    else if (viewName == "Points") {

        var fromDate = $("#fromDate").val();
        var toDate = $("#toDate").val();
        uri = '/Dashboard/GetRetailerPointsLedgerView?filterType=' + id + '&filterValue=' + value + '&fromDate=' + fromDate + '&toDate=' + toDate;
    }
    else if (viewName == "Product") {
        uri = '/Dashboard/GetProductView?filterType=' + id + '&filterValue=' + value;
    }
    else if (viewName == "promotionStatus") {
        uri = '/Dashboard/GetPromotionView?filterType=' + id + '&filterValue=' + value;
    }
    else if (viewName == "MediaCategory") {
        uri = '/Dashboard/GetMediaCategoryView?filterType=' + id + '&filterValue=' + value;
    }
    else if (viewName == "MediaGalary") {
        uri = '/Dashboard/GetGallaryView?filterType=' + id + '&filterValue=' + value;
    }
    else if (viewName == "Order") {
        uri = '/Dashboard/GetOrders?filterType=' + id + '&filterValue=' + value;
    }
    else if (viewName == "Notification") {
        uri = '/Dashboard/GetNotificationListView?filterType=' + id + '&filterValue=' + value;
    }


    uri = uri.trim().replace(/ /g, '%20');
    window.location.href = (uri);
    return false;
}

function SaveAppVersion() {
    $("#error").html("");
    var detail = $('#versionForm').serialize();
    $(".preloader").show();
    $.ajax({
        url: 'SaveAppVersionDetail',
        type: 'Post',
        datatype: 'Json',
        data: detail
    }).done(function (result) {
        $("#error").html(result);
        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function abortRead() {
    reader.abort();
}

function errorHandler(evt) {
    switch (evt.target.error.code) {
        case evt.target.error.NOT_FOUND_ERR:
            alert('File Not Found!');
            break;
        case evt.target.error.NOT_READABLE_ERR:
            alert('File is not readable');
            break;
        case evt.target.error.ABORT_ERR:
            break; // noop
        default:
            alert('An error occurred reading this file.');
    };
}

function updateProgress(evt) {
    // evt is an ProgressEvent.
    if (evt.lengthComputable) {
        var percentLoaded = Math.round((evt.loaded / evt.total) * 100);
        // Increase the progress bar length.
        if (percentLoaded < 100) {
            //progress.style.width = percentLoaded + '%';
            //progress.textContent = percentLoaded + '%';
        }
    }
}

function handleFileSelect(evt) {
    // Reset progress indicator on new file selection.
    //progress.style.width = '0%';
    //progress.textContent = '0%';

    reader = new FileReader();
    reader.onerror = errorHandler;
    reader.onprogress = updateProgress;
    reader.onabort = function (e) {
        alert('File read cancelled');
    };
    reader.onloadstart = function (e) {
        //document.getElementById('progress_bar').className = 'loading';
    };
    reader.onload = function (e) {
        // Ensure that the progress bar displays 100% at the end.
        //progress.style.width = '100%';
        //progress.textContent = '100%';
        //setTimeout("document.getElementById('progress_bar').className='';", 2000);
    }

    // Read in the image file as a binary string.
    //reader.readAsArrayBuffer(evt.target.files[0]);

    reader.readAsDataURL(evt.target.files[0]);
}

/***********End upload image*************/

function SaveMessageDetailImage() {
    $(".preloader").show();
    var path = document.getElementById('files').value;
    if (path == null || path == "") {
        SaveMessageDetail();
    }
    else {
        var n = path.lastIndexOf('\\');
        var selectedFileName = path.substring(n + 1);
        res = reader.result;
        $(".preloader").show();
        $.ajax({
            url: '/Dashboard/SaveImage',
            type: 'Post',
            datatype: 'Json',
            data: { data: res, fileName: selectedFileName }, //your string data
        }).done(function (result) {
            if (result != null) {
                document.getElementById("imageFilePath").value = result;
            }

            SaveMessageDetail();
            //$(".preloader").hide();
        }).fail(function (error) {
            $("#error").html(error.statusText);
            $(".preloader").hide();
        });

        return false;
    }

    return false;
}

function SaveMessageDetail() {

    var data = $('#addMessageForm').serialize();
    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/SaveMessageDetail', // update function here
        type: 'Post',
        datatype: 'Json',
        data: data
    }).done(function (result) {
        $("#error").html(result);
        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function SaveBannerImage() {
    $(".preloader").show();
    var path = document.getElementById('files').value;
    if (path == null || path == "") {
        SaveBannerDetail();
    }
    else {
        var n = path.lastIndexOf('\\');
        var selectedFileName = path.substring(n + 1);
        res = reader.result;
        $(".preloader").show();
        $.ajax({
            url: '/Dashboard/SaveImage',
            type: 'Post',
            datatype: 'Json',
            data: { data: res, fileName: selectedFileName }, //your string data
        }).done(function (result) {
            if (result != null) {
                document.getElementById("imageFilePath").value = result;
            }

            SaveBannerDetail();
            //$(".preloader").hide();
        }).fail(function (error) {
            $("#error").html(error.statusText);
            $(".preloader").hide();
        });

        return false;
    }

    return false;
}

function SaveBannerDetail() {

    var data = $('#addBannerForm').serialize();
    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/SaveBannerForm', // update function here
        type: 'Post',
        datatype: 'Json',
        data: data
    }).done(function (result) {
        $("#error").html(result);
        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function SaveFestivePoint() {

    var data = $('#festivePointForm').serialize();
    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/SaveFestivePointDetail',
        type: 'Post',
        datatype: 'Json',
        data: data
    }).done(function (result) {
        $("#error").html(result);
        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function SaveGallaryImagePath() {
    $(".preloader").show();
    var path = document.getElementById('files').value;
    if (path == null || path == "") {
        SaveGallaryDetail();
    }
    else {
        var n = path.lastIndexOf('\\');
        var selectedFileName = path.substring(n + 1);
        document.getElementById("DisplayName").value = selectedFileName;
        res = reader.result;
        $(".preloader").show();
        $.ajax({
            url: '/Dashboard/SaveImage',
            type: 'Post',
            datatype: 'Json',
            data: { data: res, fileName: selectedFileName }, //your string data
        }).done(function (result) {
            if (result != null) {
                document.getElementById("imageFilePath").value = result;
            }

            SaveGallaryDetail();
            //$(".preloader").hide();
        }).fail(function (error) {
            $("#error").html(error.statusText);
            $(".preloader").hide();
        });

        return false;
    }

    return false;
}

function SaveNotificationImagePath() {
    $(".preloader").show();
    var path = document.getElementById('files').value;
    if (path == null || path == "") {
        AddAndSendNotification();
    }
    else {
        var n = path.lastIndexOf('\\');
        var selectedFileName = path.substring(n + 1);
        //document.getElementById("DisplayName").value = selectedFileName;
        res = reader.result;
        $(".preloader").show();
        $.ajax({
            url: '/Dashboard/SaveImage',
            type: 'Post',
            datatype: 'Json',
            data: { data: res, fileName: selectedFileName }, //your string data
        }).done(function (result) {
            if (result != null) {
                document.getElementById("imageFilePath").value = result;
            }

            AddAndSendNotification();
            //$(".preloader").hide();
        }).fail(function (error) {
            $("#error").html(error.statusText);
            $(".preloader").hide();
        });

        return false;
    }

    return false;
}

function SaveGallaryDetail() {

    var data = $('#gallaryForm').serialize();
    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/SaveGallaryDetail',
        type: 'Post',
        datatype: 'Json',
        data: data
    }).done(function (result) {
        $("#error").html(result);
        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function SaveImagePath() {
    $(".preloader").show();
    var path = document.getElementById('files').value;
    if (path == null || path == "") {
        SaveProductDetail();
    }
    else {
        var n = path.lastIndexOf('\\');
        var selectedFileName = path.substring(n + 1);
        $(".preloader").show();
        $.ajax({
            url: '/Dashboard/SaveImage',
            type: 'Post',
            datatype: 'Json',
            data: { data: reader.result, fileName: selectedFileName }, //your string data
        }).done(function (result) {
            if (result != null) {
                document.getElementById("imageFilePath").value = result;
            }

            SaveProductDetail();
            //$(".preloader").hide();
        }).fail(function (error) {
            $("#error").html(error.statusText);
            $(".preloader").hide();
        });

        return false;
        //-----------
        //var n = path.lastIndexOf('\\');
        //var selectedFileName = path.substring(n + 1);
        //$.ajax({
        //    url: '/Dashboard/GetImage',
        //    dataType: 'json',
        //    type: 'POST',
        //    data: { data: reader.result, fileName: selectedFileName }, //your string data
        //    success: function (response) {
        //        //$("#imageFilePath").html(response);
        //        if (response != null) {
        //            document.getElementById("imageFilePath").value = response;
        //        }

        //        SaveProductDetail();
        //    }
        //});
    }

    return false;
}

function SaveProductDetail() {

    var data = $('#productForm').serialize();
    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/SaveProductImageDetail',
        type: 'Post',
        datatype: 'Json',
        data: data
    }).done(function (result) {
        $("#error").html(result);
        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function UpdateProductStatus(prodId) {

    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/UpdateProductStatus',
        type: 'Post',
        datatype: 'Json',
        data: { productId: prodId }
    }).done(function (result) {
        if (result.Status == false) {
            $("#error").html(result.ResponseValue);
        }
        else {
            $("#" + prodId).html("");
            $("#" + prodId).html(result.ResponseValue);
        }

        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function UpdateOrderStatus() {

    var data = $('#orderStatus').serialize();
    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/UpdateStatus',
        type: 'Post',
        datatype: 'Json',
        data: data
    }).done(function (result) {
        if (result.indexOf("Error : ") >= 0) {
            $("#errorDetail").html(result);
        }
        else {
            $(".ui-dialog-titlebar-close").click();
            $("#responseContainer").html("");
            $("#responseContainer").html(result);
        }

        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function UpdateRetailerPrmotionImageStatus(imageId, approveStatus) {
    $("#errorMessage").html("");
    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/UpdateRetailerImageStatus',
        type: 'Post',
        datatype: 'Json',
        data: { Id: imageId, status: approveStatus }
    }).done(function (result) {
        if (result == null) {
            $("#responseMessage").html("Something went wrong, Please try again later.");
        }
        else {
            $("#responseMessage").html("");
            $("#responseMessage").html(result.ResponseValue);
        }


        $("#statusLink").click();
        //$("#updateImageStatusDialog").dialog({
        //    dialogClass: "noclose"
        //});

        //$("#OkButton").click(function () {
        //    $(".ui-dialog-titlebar-close").click();
        //});
        $(".preloader").hide();

    }).fail(function (error) {
        $("#responseMessage").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function SavePromotionImagePath() {
    $(".preloader").show();
    var path = document.getElementById('files').value;
    if (path == null || path == "") {
        SavePromotionDetail();
    }
    else {
        var n = path.lastIndexOf('\\');
        var selectedFileName = path.substring(n + 1);
        $(".preloader").show();
        $.ajax({
            url: '/Dashboard/SaveImage',
            type: 'Post',
            datatype: 'Json',
            data: { data: reader.result, fileName: selectedFileName }, //your string data
        }).done(function (result) {
            if (result != null) {
                document.getElementById("imageFilePath").value = result;
            }

            SavePromotionDetail();
            //$(".preloader").hide();
        }).fail(function (error) {
            $("#error").html(error.statusText);
            $(".preloader").hide();
        });

        return false;
    }

    return false;
}

function SavePromotionDetail() {

    var data = $('#promotionForm').serialize();
    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/SavePromotionImageDetail',
        type: 'Post',
        datatype: 'Json',
        data: data
    }).done(function (result) {
        $("#error").html(result);
        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function UpdateProductStatus(prodId) {

    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/UpdateProductStatus',
        type: 'Post',
        datatype: 'Json',
        data: { productId: prodId }
    }).done(function (result) {
        if (result.Status == false) {
            $("#error").html(result.ResponseValue);
        }
        else {
            $("#" + prodId).html("");
            $("#" + prodId).html(result.ResponseValue);
            $("#data" + prodId).html(result.ResponseValue);
        }

        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function UpdatePromotionRetailerEntrySatus(promoId, retId, action) {

    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/UpdatePromotionRetailerEntrySatus',
        type: 'Post',
        datatype: 'Json',
        data: { PromoId: promoId, RetailerId: retId, operation: action }
    }).done(function (result) {
        $(".preloader").hide();
        ShowDialogue(result, promoId);

        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);

    });

    return false;
}

function UpdatePromotionStatus(promoId) {

    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/UpdatePromotionStatus',
        type: 'Post',
        datatype: 'Json',
        data: { Id: promoId }
    }).done(function (result) {
        if (result.Status == false) {
            $("#error").html(result.ResponseValue);
        }
        else {
            $("#" + promoId).html("");
            $("#" + promoId).html(result.ResponseValue);
            $("#data" + promoId).html(result.ResponseValue);
        }

        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function UpdateUploadedMediaStatus(detailId) {

    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/UpdateUploadedMediaStatus',
        type: 'Post',
        datatype: 'Json',
        data: { Id: detailId }
    }).done(function (result) {
        if (result.Status == false) {
            $("#error").html(result.ResponseValue);
        }
        else {
            $("#" + detailId).html("");
            $("#" + detailId).html(result.ResponseValue);
            $("#data" + detailId).html(result.ResponseValue);
        }

        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function UpdateFestivePointStatus(detailId) {

    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/UpdateFestivePointStatusDetail',
        type: 'Post',
        datatype: 'Json',
        data: { Id: detailId }
    }).done(function (result) {
        if (result.Status == false) {
            $("#error").html(result.ResponseValue);
        }
        else {
            $("#" + detailId).html("");
            $("#" + detailId).html(result.ResponseValue);
            $("#data" + detailId).html(result.ResponseValue);
        }

        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function UpdateMessageStatus(detailId) {

    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/UpdateMessageStatusDetail',
        type: 'Post',
        datatype: 'Json',
        data: { Id: detailId }
    }).done(function (result) {
        if (result.Status == false) {
            $("#error").html(result.ResponseValue);
        }
        else {
            $("#" + detailId).html("");
            $("#" + detailId).html(result.ResponseValue);
            $("#data" + detailId).html(result.ResponseValue);
        }

        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function UpdateBannerStatus(detailId) {

    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/UpdateBannerStatusDetail',
        type: 'Post',
        datatype: 'Json',
        data: { Id: detailId }
    }).done(function (result) {
        if (result.Status == false) {
            $("#error").html(result.ResponseValue);
        }
        else {
            $("#" + detailId).html("");
            $("#" + detailId).html(result.ResponseValue);
            $("#data" + detailId).html(result.ResponseValue);
        }

        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function UpdateMediaCategoryStatus(detailId) {

    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/UpdateMediaCategoryStatus',
        type: 'Post',
        datatype: 'Json',
        data: { Id: detailId }
    }).done(function (result) {
        if (result.Status == false) {
            $("#error").html(result.ResponseValue);
        }
        else {
            $("#" + detailId).html("");
            $("#" + detailId).html(result.ResponseValue);
        }

        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function SaveMediaCategoryDetail() {

    var data = $('#mediaForm').serialize();
    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/SaveMediaCategoryDetail',
        type: 'Post',
        datatype: 'Json',
        data: data
    }).done(function (result) {
        $("#error").html(result);
        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function AddAndSendNotification() {

    var data = $('#notificationForm').serialize();
    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/AddAndSendNotificationDetail',
        type: 'Post',
        datatype: 'Json',
        data: data
    }).done(function (result) {
        $("#error").html(result);
        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function DeleteNotification(notiId) {

    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/DeleteNotification',
        type: 'Post',
        datatype: 'Json',
        data: { Id: notiId }
    }).done(function (result) {

        if (result == null) {
            $("#responseMessage").html("Something went wrong, Please try again later.");
        }
        else {
            $("#responseMessage").html("");
            $("#responseMessage").html(result.ResponseValue);
        }

        $("#deleteLink").click();
        $("#deleteBtn").click(function () {
            window.location.href = '/Dashboard/GetNotificationListView';
        });

        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}
