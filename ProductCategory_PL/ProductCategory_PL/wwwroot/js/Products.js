
//async function encryptId(plainText) {
//    const key = await crypto.subtle.importKey(
//        "raw",
//        new TextEncoder().encode("b14ca5898a4e4133bbce2ea2315a1916"), // Your encryption key
//        { name: "AES-CBC" },
//        false,
//        ["encrypt"]
//    );

//    const iv = new Uint8Array(16); // Initialization vector
//    const encrypted = await crypto.subtle.encrypt(
//        { name: "AES-CBC", iv: iv },
//        key,
//        new TextEncoder().encode(plainText)
//    );

//    return btoa(String.fromCharCode(...new Uint8Array(encrypted)));
//}

async function renderDetailsButton(row) {
    const encryptedId = await encryptId(row.id.toString());
    return `
        <a href="/product/Details/${encryptedId}" class="btn btn-sm btn-warning" title="Details">
            <i class="fas fa-info-circle"></i> Details
        </a>
    `;
}


document.addEventListener("DOMContentLoaded", InitializeDatatable);

// For Display Data 
function InitializeDatatable() {
    var table = $('#kt_datatable');
    var userRole;
    table.dataTable({
        responsive: true,
        processing: true,
        serverSide: false,
        pageLength: 10,
        pagination: true,
        dom: "<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
        language: {
            search: "Search:",
            lengthMenu: "Display _MENU_ records per page",
            zeroRecords: "No matching records found",
            info: "Showing _START_ to _END_ of _TOTAL_ entries",
            infoEmpty: "No entries to show",
            paginate: {
                previous: "Previous",
                next: "Next"
            }
        },
        ajax: {
            url: '/product/GetAll',
            type: 'GET',
            dataSrc: function (d) {
              

                userRole = d.role;
                return d.data.result;
            },
            error: function (e) {
                alert('Error fetching data: ' + e);
            }
        },
        columns: [
            { data: 'Num', responsivePriority: 0 },
            {
                data: 'pictureUrl',
                title: 'Image',
                render: function (data, type, row) {
                    if (data) {
                        return '<img src="/Images/products/' + data + '" alt="Product Image" style="width: 50px; height: auto;" />';
                    }
                    return 'No Image';
                }
            },
            { data: 'name', title: 'Name' },
            { data: 'price', title: 'Price' },
            { data: 'productCategory.name', title: 'Product Category' },
            { data: 'productBrand.name', title: 'Product Brand' },
            { data: 'Actions', responsivePriority: -1, className: 'actions' }
        ],
        columnDefs: [
            {
                targets: 0,
                title: 'Number',
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                targets: -1,
                title: 'Actions',
                orderable: false,
                render: function (data, type, row) {
                    if (userRole) {
                        return `
                            <div class="btn-group" role="group" aria-label="Actions">
                                <a href="/product/AddUpdate/${row.id}" class="btn btn-sm btn-primary" title="Update">
                                    <i class="fas fa-edit"></i> Update
                                </a>
                                <a href="javascript:void(0);" onclick="deleteRow('${row.id}')" class="btn btn-sm btn-danger" title="Delete">
                                    <i class="fas fa-trash-alt"></i> Delete
                                </a>
                                <a href="/product/Details/${row.id}" class="btn btn-sm btn-warning" title="Details">
                                    <i class="fas fa-info-circle"></i> Details
                                </a>
                            </div>
                        `;
                    } else {
                        return `
                            <a href="/product/Details/${row.id}" class="btn btn-sm btn-warning" title="Details">
                                <i class="fas fa-info-circle"></i> Details
                            </a>
                        `;
                    }
                }
            }
        ],
        order: [[0, "asc"]],
    });

    $('#kt_datatable_search_query').on('keyup', function () {
        table.DataTable().search(this.value).draw();
    });
}


//For Delete Product
function deleteRow(id) {
    Swal.fire({
        title: 'Product Delete',
        text: 'Are you want delete this product ?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "POST",
                url: '/product/Delete',
                data: { "id": id },
                success: function (data) {
                    if (data.isValid) {
                        toastr.success(data.message , '');
                        $('#kt_datatable').DataTable().ajax.reload();
                    } else {
                        toastr.error(data.message, '');
                    }
                },
                error: function (err) {
                    toastr.error('An error occurred while deleting the product');
                }
            });
        }
    });



}