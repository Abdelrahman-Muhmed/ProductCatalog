document.addEventListener("DOMContentLoaded", InitializeDatatable);

function InitializeDatatable() {
    var table = $('#kt_datatable');

    table.dataTable({
        responsive: true,
        processing: true,
        serverSide: false,
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
            dataSrc: '',
            error: function (e) {
                alert('Error fetching data: ' + e);
            },
            dataSrc: function (d) {
                console.log(d)
                return d.data.result;
            }
        },
        columns: [
            { data: 'Num', responsivePriority: 0 },
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
                    console.log(row)
                    return `
                            <div class="btn-group">
                                <a href="/Complaint/Edit/${row.Id}" class="btn btn-sm btn-primary" title="Edit">
                                    Update
                                </a>

                                <a href="javascript:void(0);" onclick="deleteRow('${row.id}')" class="btn btn-sm btn-danger" title="Delete">
                                    
                                    Delete
                                </a>
                            </div>
                        `;
                }
            }
        ],
        order: [[0, "asc"]],
    });

    $('#kt_datatable_search_query').on('keyup', function () {
        table.DataTable().search(this.value).draw();
    });
}






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
                        toastr.success('The product has been deleted');
                        $('#kt_datatable').DataTable().ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                },
                error: function (err) {
                    toastr.error('An error occurred while deleting the product');
                }
            });
        }
    });



}