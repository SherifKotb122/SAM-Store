// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// using the filtering at the dashboard table

    
$(function () {
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
});

showPopUp = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            $('#form-modal').modal('show');
        }
    })
}

//$(document).ready(function () {
//    $('#productFrom').submit(function (e) {
//        e.preventDefault();
//        console.log(e);
//        debugger;
//        /*  var $form = $(this);*/
//        var form = document.getElementById("productFrom");
//        try {
//            $.ajax({
//                type: 'POST',
//                url: "Products/AddOrEdit",
//                data: new FormData(form),
//                contentType: false,
//                processData: false,
//                success: function (res) {

//                    $("#view-all").html(res);
//                    $('#form-modal .modal-body').html('');
//                    $('#form-modal .modal-title').html('');
//                    $('#form-modal').modal('hide');
//                    $.notify('Submitted successfuly', { globalPosition: 'top center', className: 'success' });

//                },
//                error: function (e) {
//                    console.log(e);
//                }
//            })

//        } catch (e) {
//            console.log(e);
//        }
//    });
//});

  // check if the input is valid using a 'valid' propertyif (!$form.valid) return false;

//jQueryAjaxPost = (e) =>{
///*return false;*/
//    e.preventDefault();
//    console.log(e);
//    var form = document.getElementById("productFrom");
//    try {
//        $.ajax({
//            type: 'POST',
//            url: form.action,
//            data: new FormData(form),
//            contentType: false,
//            processData: false,
//            success: function (res) {
               
//                    $("#view-all").html(res);
//                    $('#form-modal .modal-body').html('');
//                    $('#form-modal .modal-title').html('');
//                $('#form-modal').modal('hide');
//                $.notify('Submitted successfuly', { globalPosition: 'top center', className: 'success' });
               
//            },
//            error: function (e) {
//                console.log(e);
//            }
//        })

//    } catch(e) {
//        console.log(e);
//    }

   
//}

function AjaxDelete(e){
    e.preventDefault();
    //const id = e.target.getAttribute("data-id");
    var form = document.getElementById("delForm");
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
              
                $("#view-all").html(res);
                $('#delete-modal .modal-body').html('');
                $('#delete-modal .modal-title').html('');
                $('#delete-modal').modal('hide');
                $.notify('Deleted successfuly', { globalPosition: 'top center', className: 'success' });
                $('#tableFilter').DataTable({
                    "scrollY": "450px",
                    "scrollCollapse": true,
                    "paging": true,
                    "select": true,
                    "ordering": true,
                    "searching": true,
                    "scrollX": false,
                    "autoWidth": true,
                    dom: 'Bfrtip',
                    buttons: [
                        {
                            extend: 'copy',
                            className: 'bg-primary',
                            text: '<i class="far fa-copy"></i> Copy'
                        }, {
                            extend: 'excel',
                            className: 'bg-primary',
                            text: '<i class="far fa-file-excel"></i> Excel'
                        }, {
                            extend: 'pdf',
                            className: 'bg-primary',
                            text: '<i class="far fa-file-pdf"></i> Pdf'
                        }, {
                            extend: 'print',
                            className: 'bg-primary',
                            text: '<i class="fas fa-print"></i> Print'
                        }

                    ]
                });

            },
            error: function (e) {
                console.log(e);
            }
        })

    } catch (e) {
        console.log(e);
    }
  
    
}

deletePopUp = (url,title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $('#delete-modal .modal-body').html(res);
            $('#delete-modal .modal-title').html(title);
            $('#delete-modal').modal('show');
        }
    })
} 

function canelDel() {
    $('#delete-modal .modal-body').html('');
    $('#delete-modal .modal-title').html('');
    $('#delete-modal').modal('hide');
}

function editDel() {
    $('#form-modal .modal-body').html('');
    $('#form-modal .modal-title').html('');
    $('#form-modal').modal('hide');
}
//function as(event){
//    e.preventDefault();
//}

$(document).ready(function () {
    $('#tableFilter').DataTable({
        "scrollY": "350px",
        "scrollCollapse": false,
        "paging": true,
        "select": true,
        "ordering": true,
        "searching": true,
        "scrollX": false,
        "autoWidth": true,
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'copy',
                className: 'bg-white text-primary butHover',
                text: '<i class="far fa-copy" style="color: #cb0c9f;"></i> Copy'
            }, {
                extend: 'excel',
                className: 'bg-white text-primary butHover',
                text: '<i class="far fa-file-excel" style="color: #cb0c9f;"></i> Excel'
            }, {
                extend: 'pdf',
                className: 'bg-white text-primary butHover',
                text: '<i class="far fa-file-pdf" style="color: #cb0c9f;"></i> Pdf'
            }, {
                extend: 'print',
                className: 'bg-white text-primary butHover',
                text: '<i class="fas fa-print icoExport" style="color: #cb0c9f;"></i> Print'
            }
            
        ]
   
    });
    //$(document).ready(function () {
    //    $('#tableFilter').DataTable();
    //});
});


//------------------Show Product Details----------------------
showDetails = (url) => {
    console.log("hi");
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $('#details-modal .modal-body').html(res);
            $('#details-modal').modal('show');
        }
    })
}

CloseModel = () => {
    $('#details-modal').modal('hide');
    $('#checkout-modal').modal('hide');
}

//-------------------Delete Wishlist-----------------------
remove = (url) => {
    $.ajax({
        method: "GET",
        url: url,
        success: function (res) {
            //debugger;
            $('#wishlist').html(res);
            ChangingWishlist();
        }
    });
}

AddToCart = (id, quantity, e) => {
 
    var btn = e.target;
    if (btn.nodeName === 'SPAN') {
        btn = btn.parentNode;
    }
    $.ajax({
        url: "Carts/AddToCart",
        type:"get",
        data: { "id": id, "quantity": quantity },
        traditional: true,
        success: function (res) {
            CloseModel();
            btn.setAttribute("disabled", "true");
            $.notify('Added successfuly', { globalPosition: 'top center', className: 'success' });
            getCartsCount();
        },
        error: function (err) {
            $.notify('You need to login first', { globalPosition: 'top center', className: 'warning' });
        }
    });
}

AddToWishlist = (id, e) => {
    e.stopPropagation();
    var btn = e.target;
    var act;
    if (btn.nodeName==='SPAN') {
        btn = btn.parentNode;
    }
    if (btn.classList.contains("btn-light") || btn.classList.contains("card-link-secondary")) {
            btn.classList.remove("btn-light");
            btn.classList.add("btn-danger");
            act = "Add";
        }
        else {
            btn.classList.remove("btn-danger");
            btn.classList.add("btn-light");
            act = "Delete";
    }
    $.ajax({
        type: 'get',
        url: "Wishlists/AddOrRemove",
        traditional: true,
        data: { "id": id, "act": act }
    });
}
AddToWishlistHome = (id, e) => {
    e.stopPropagation();
    var btn = e.target;
    var act = "Add";
   
    $.ajax({
        type: 'get',
        url: "Wishlists/AddOrRemove",
        traditional: true,
        data: { "id": id, "act": act }
    });
}

function prevent(e) {
    e.preventDefault();
}

removeFromCart = (id, e) => {
    e.preventDefault();
    $.ajax({
        type:"get",
        traditional: true,
        url: "Carts/RemoveFromCart",
        data: { "id": id },
        success: function (res) {
            $("#newCart").html(res);
            Changing();
            getCartsCount();
        },
        error: function (err) {
            console.log(err)
        }
    })
}

function IncOrDec(e, price, status, id) {
    var Temp = document.getElementById("subTotal");
    //var Total = document.getElementById("total");
    var myInput = e.target.parentNode.querySelector('input[type=number]');
    var flag=false;

    if (status == "minus") {
        if (myInput.value > 1) {
            e.target.parentNode.querySelector('input[type=number]').stepDown();
            Temp.innerHTML = Math.round((parseFloat(Temp.innerHTML) - price)*100, 2)/100;
            //Total.innerHTML = (parseFloat(Total.innerHTML) - price);
            flag = true;
        }
    }

    else {
        if (parseInt(myInput.value) < parseInt(myInput.max)) {
            e.target.parentNode.querySelector('input[type=number]').stepUp();
            Temp.innerHTML = Math.round((parseFloat(Temp.innerHTML) + price)*100, 2)/100;
            //Total.innerHTML = (parseFloat(Total.innerHTML) + price);
            flag = true;
        }
    }
    if (flag) {

        fetch("http://shirleyomda-001-site1.etempurl.com/odata/Carts(" + id + ")",
              {
                  method: 'PATCH',
                  headers: {
                 'Content-Type': 'application/json'
            },
                  body: JSON.stringify({ "quantity": parseInt(myInput.value) }),
        });
    }
}

MoveToWishlistFromCart = (sID, pID, e) => {
    AddToWishlist(pID, e);
    removeFromCart(sID, e);
    Changing();
    $.notify('Moved to wishlist successfuly', { globalPosition: 'top center', className: 'success' });
}

function changeTotal(e){
    const shippingCost = e.target.options[e.target.selectedIndex].value;
    const tempTotal = $("#tempTotal").html();
    //console.log($("#tempTotal").html());
    $("#shipping").html(shippingCost);
    $("#totalt").html((parseFloat(shippingCost) + parseFloat(tempTotal)));

    //Validation
    let shippingID = e.target.options[e.target.selectedIndex].getAttribute("id");

    if (shippingID == null) {
        $("#goverVal").show();
    }
    else {
        $("#goverVal").hide();
    }
}

function ConfirmPayment(e) {
    const itemCount = parseInt($("#itemCount").html());
    if (itemCount==0) {
    $.notify('Your cart is empty, Please continue shopping', { globalPosition: 'top center', className: 'danger' });
    }
    else {

    let total = parseFloat($("#subTotal").html());
    $.ajax({
        type: 'get',
        url: "Carts/Purchase",
        traditional: true,
        data: { "total": total },
        success: function (res) {
            $('#checkout-modal .modal-body').html(res);
            $('#checkout-modal').modal('show');
        },
        error: function (err) {
            console.log(err);
        }
    });
    //Modal carry all things(payment details of address)
    }
}

$(document).ready(function () {
    Changing();
    //const itemCount = parseInt($("#itemCount").html());
    //const myBtn = document.getElementById("btnCheckout");
    //if (itemCount == 0) {
    //    myBtn.setAttribute("disabled", "true");
    //    $("#whenEmpty").show();
    //    var scrollableDiv = document.getElementById("scrollDiv");
    //    scrollableDiv.classList.remove("scrollDiv");
    //    var cartNote = document.getElementById("cartNote");
    //    cartNote.classList.add("hide");
    //}
})


function Changing() {
    const itemCount = parseInt($("#itemCount").html());
    const myBtn = document.getElementById("btnCheckout");
    if (itemCount == 0) {
        myBtn.setAttribute("disabled", "true");
        $("#whenEmpty").show();
        var scrollableDiv = document.getElementById("scrollDiv");
        scrollableDiv.classList.remove("scrollDiv");
        var cartNote = document.getElementById("cartNote");
        cartNote.classList.add("hide");
    }
}

$(document).ready(function () {
    ChangingWishlist();
})

function ChangingWishlist() {
    const itemCount = parseInt($("#wishlistCount").html());
    if (itemCount == 0) {
        $("#whenEmpty").show();
    }
}

function checkOut(e) {
    
    //Address => Payment => Bill => BillProduct
    //e.preventDefault();
    //Address Table Data
    let shippingList = document.getElementById("shippingAddress");
    let shippingID = shippingList.options[shippingList.selectedIndex].getAttribute("id");
    let phone = parseInt($("#phone").val());
    let addressDetails = $("#address").val();

    //Bill
    //Payment
    var payMethod = $("input[name='pay']:checked").attr('id');

    let tempTotal = parseFloat($("#tempTotal").html());
    let total = parseFloat($("#totalt").html());

    if (shippingID != null && !isNaN(phone) && addressDetails != "" && payMethod != null) {
        $("#goverVal").hide();
        $("#phoneVal").hide();
        $("#addVal").hide();
        $("#payVal").hide();

        if (payMethod == 1) {
            window.location.href = "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_xclick&amount=" + (total / 15.67) + "&business=sherif_kotb122@yahoo.com&item_name=products&return=Page";
        }
        else {
        $.ajax({
            url: "Bills/Index",
            type: 'get',
            traditional: true,
            data: { "shippingID": shippingID, "phone": phone, "addressDetails": addressDetails, "paymentID": payMethod, "tempTotal": tempTotal, "total": total },
            success: function (res) {
                CloseModel();
                $.notify('Order Submitted Successfully', { globalPosition: 'top center', className: 'success' });

                //window.location.href = 'Url.Action("BillDetails", "Bills", new { id: res.id } )';
                //$("#loaderbody").removeClass('loaderbody');
                //$("#loadload").removeClass('loader'); 
                window.location.href = 'Bills/BillDetails/' + res.id;

                //$("#newCart").html(res);
                //$("#loaderbody").addClass('hide');
                //Search For passing Form Mn Here !!
                //Render partial view or full view ????
            }
        });
        }
    }
    else {
        e.preventDefault();
        if (shippingID == null) {
            $("#goverVal").show();
        }
        if (isNaN(phone)) {
            $("#phoneVal").show();
        }
        if (addressDetails == "") {
            $("#addVal").show();
        }
        if (payMethod == null) {
            $("#payVal").show();
        }
    }
}
//function printClick(){
//    $("#loaderbody").addClass('hide');

//}



//$(document).ready(function () {
//    $('#openTableUser').DataTable({
//        "scrollY": "350px",
//        "scrollCollapse": false,
//        "paging": true,
//        "select": true,
//        "ordering": true,
//        "searching": true,
//        "scrollX": false
//       /* "autoWidth": true*/
       

//    });
//});
$(document).ready(function () {
    $('#closeTableUser').DataTable({
        "scrollY": "350px",
        "scrollCollapse": false,
        "paging": true,
        "select": true,
        "ordering": true,
        "searching": true,
        "scrollX": false
        /*   "autoWidth": true*/


    });
    //$('#openTableUser').DataTable({
    //    "scrollY": "350px",
    //    "scrollCollapse": false,
    //    "paging": true,
    //    "select": true,
    //    "ordering": true,
    //    "searching": true,
    //    "scrollX": false,
    //    "autoWidth": true
    //    /*   "autoWidth": true*/


    //});
});
$(document).ready(function () {
    $('#openTableUser').DataTable({
        "scrollY": "350px",
        "scrollCollapse": false,
        "paging": true,
        "select": true,
        "ordering": true,
        "searching": true,
        "scrollX": false
        


    });
});

/*>>>>>>>>>>>>>>>>>>>dobule click on row detect<<<<<<<<<<<<<<<<<*/
$(document).ready(function () {
    var table = $('#openTableUser').DataTable();

    $('#openTableUser tbody').on('dblclick', 'tr', function () {
        var data = table.row(this).data();
        //alert('You clicked on ' + data[0] + '\'s row');
        window.location.href = 'Bills/BillDetails/' + data[0];
    });
});
$(document).ready(function () {
    var table = $('#closeTableUser').DataTable();

    $('#closeTableUser tbody').on('dblclick', 'tr', function () {
        var data = table.row(this).data();
        //alert('You clicked on ' + data[0] + '\'s row');
        window.location.href = 'Bills/BillDetails/' + data[0];
    });
});

$('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
    $($.fn.dataTable.tables(true)).DataTable()
        .columns.adjust();
});

    
    function ValidatePhone() {
    let phone = parseInt($("#phone").val());
    if (isNaN(phone)) {
        $("#phoneVal").show();
    }
    else {
        $("#phoneVal").hide();
    }
}

    function ValidateAdd() {
        let addressDetails = $("#address").val();
        if (addressDetails == "") {
            $("#addVal").show();
        }
        else {
            $("#addVal").hide();
        }
    }

function AddReview(e) {
    const review = e.target.previousElementSibling.value;
    const id = e.target.parentNode.parentNode.parentNode.parentNode.getAttribute("data-id");
    const rate = e.target.parentNode.parentNode.parentNode.parentNode.getAttribute("data-value");

    $.ajax({
        type: 'get',
        url: "Reviews/AddReview",
        traditional: true,
        data: { "id": parseInt(id), "review": review, "rate": parseInt(rate) },
        success: function (res) {
            $('#toReview').html(res);
            $.notify('thanks for your review', { globalPosition: 'top center', className: 'success' });
        },
        error: function (err) {
            console.log(err);
        }
    });
}
    function ValidatePay() {
    var payMethod = $("input[name='pay']:checked").attr('id');
    if (payMethod == null) {
        $("#payVal").show();
    }
    else {
        $("#payVal").hide();
    }
}

function changeImage(e) {
    const myImage = document.getElementById("masterImage");
    
    myImage.setAttribute("src", e.target.getAttribute("src"));
}

function getCartsCount() {
    var badge = document.getElementById("spanBadge");
    var userID = badge.getAttribute("data-id");
    fetch("http://shirleyomda-001-site1.etempurl.com/odata/Carts?$filter=userID eq '" + userID + "'",
    {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    }).then((res) => res.json())
    .then((data) => {
        if (data.value.length > 0) {
            badge.innerHTML = data.value.length;
            badge.hidden = false;
        }
    });
}

$(document).ready(function () {
    getCartsCount();
});