var getUrl = window.location;
var baseUrl = getUrl.protocol + "//" + getUrl.host + "/";

var ivalidator = $("#deffer-form").kendoValidator().data("kendoValidator");

var docName = "";
var fileName = "";
var fileFormat = "";
var defferNo = 0;

$(document).ready(function () {
    //alert(baseUrl);
    
    $("#docDataInput").kendoDatePicker();
    

    $("#branch_name").kendoDropDownList({
        autoBind: true,
        optionLabel: "Select Branch",
        dataTextField: "branch_name",
        dataValueField: "branch_code",
        dataSource: {
            transport: {
                read: {
                    url: baseUrl + "api/Branches",
                    dataType: "json"   //reading data
                    
                }
            }
        }
    }).data("kendoDropDownList");

    // create DropDownList from input HTML element
    var doc_list = $("#doc_name").kendoDropDownList({
        autoBind: false,
        optionLabel: "Select Document...",
        dataTextField: "docName",
        dataValueField: "docId",
    }).data("kendoDropDownList");


    $("#accountNumber").blur(function () {
        
        var accountNo = $(this).val()
        var product = accountNo.substring(0, 3);

        //alert(product);

        doc_list.setDataSource({
            transport: {
                read: {
                    url: baseUrl + "LoanDocumentsFetch/ProdDocs/" + product,
                    dataType: "json"   //reading data

                }
            }
        });

        dataSourceR = new kendo.data.DataSource({
            type: "json",
            transport: {
                read: baseUrl + "LoanDocumentsFetch/defferDocsGrid?branchID=0002&accNo=62000000014"
            },
            pageSize: 10,
            schema: {

                model: {
                    fields: {
                        docSubmitDate: {
                            type: "date",
                            parse: function (docSubmitDate) {
                                var d = kendo.toString(kendo.parseDate(docSubmitDate), 'dd MMM yyyy')
                                return d;
                            }

                        },
                        docDefferDate: {
                            type: "date",
                            parse: function (docDefferDate) {
                                var openDate = kendo.toString(kendo.parseDate(docDefferDate), 'dd MMM yyyy')
                                return openDate;
                            }

                        }
                    }
                }
            }
        });
        var grid = $("#defferDocList").data("kendoGrid");
        grid.setDataSource(dataSourceR);

    });

    var emp_list=$("#emp_name").kendoDropDownList({
        autoBind: false,
        optionLabel: "Select Employee",
        dataTextField: "EmpName",
        dataValueField: "EmpId",
    }).data("kendoDropDownList");



    $("#branch_name").blur(function () {

        var branchID = $(this).val()
        
       // alert(branchID);

        emp_list.setDataSource({
            transport: {
                read: {
                    url: baseUrl + "api/Employees?branchID=" + branchID,
                    dataType: "json"   //reading data

                }
            }
        });
    });


    //$("#filesArea").hide();

    function onUpload(e) {
        docName = $("#branch_name").val() + $("#accountNumber").val() + $("#doc_name").val();
        //alert(docName);
        e.sender.options.async.saveUrl = baseUrl + "/FileUpload/save?id=" + docName;

        fileName = e.files[0].name;
        fileFormat = fileName.substr((fileName.lastIndexOf('.') + 1));

        alert(fileName + "  And " + fileFormat);
    }

    var fileUploader = $("#files").kendoUpload({
        enabled: false,
        multiple: false,
        async: {
             saveUrl: baseUrl + "/FileUpload/save?id=",
             removeUrl: "remove",
            autoUpload: true
        },
        upload: onUpload
    }).data("kendoUpload");


    $("#isDocProvided").click(function () {
        if (this.checked) {

            fileUploader.enable();
            defferNo = 0;
        }

        else {
            defferNo = 1;
            fileUploader.disable();
        }
    });


    $("#defferDocList").kendoGrid({
        dataSource: "",
        groupable: false,
        sortable: true,
        selectable: true, 

        columns: [{
            field: "branchCode",
            title: "Branch",
            width: 50
            },
            {
                field: "accountNumber",
                title: "Account",
                width: 100
            },
            {
                field: "docName",
                title: "Document Name",
                width: 200
            },
            {
                field: "status",
                title: "Status",
                width: 50
            },
            {
                field: "docSubmitDate",
                title: "Submited",
                width: 100
            },
            {
                field: "docDefferDate",
                title: "Deffered",
                width: 100
            },
            {
                field: "EmpName",
                title: "EmpName"
            }],

        height: 250,
        pageable: {
            refresh: true,
            pageSizes: true,
            buttonCount: 5
        }
        
    }).data("kendoGrid");

    var staticNotification = $("#staticNotification").kendoNotification({
        appendTo: "#appendto",
        autoHideAfter: 6000,
        button: true,
        hideOnClick: true

    }).data("kendoNotification");

    $('#Save').click(function (e) {
        var today = new Date();
        var dateToday = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();

        var branch = $("#branch_name").val();
        var accountNo = $("#accountNumber").val();
        var document = $("#doc_name").val();
        var isDocProvided = $("#isDocProvided").val();
        var empID = $("#emp_name").val();
        //var submitDate = new Date();
        var defferDate = $("#docDataInput").val();
        var comments = $("#comments").val();


        if (ivalidator.validate()) {
            //alert("Data");

            $.ajax({
                url: baseUrl + "api/LoanDeferralHists",
                type: 'POST',
                dataType: 'json',
                data: {
                    docId: document,
                    defferNo: defferNo,
                    branchCode: branch,
                    accountNumber: accountNo,
                    status: 1,
                    docSubmitDate: dateToday,
                    docDefferDate: defferDate,
                    rerponsibleEmpId: empID,
                    addedBy: 305,
                    addedDate: dateToday,
                    fileName: fileName,
                    fileLink: docName + "." + fileFormat,
                    comments: comments,
                    authStatus: "U"

                },
                success: function (data) {
                    alert("Save");
                    $("#defferDocList").data("kendoGrid").dataSource.read();
                    staticNotification.getNotifications().remove();
                    staticNotification.show("Visit related information saved successfully!");
                },
                error: function (xhr, errorType, exception) {
                    alert("Error");
                    var responseText = jQuery.parseJSON(xhr.responseText);
                    staticNotification.getNotifications().remove();
                    staticNotification.show(responseText.Message, "warning");
                }
            });
        }

    });



});