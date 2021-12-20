var dataTable;
$(document).ready(function () {
    loadData();
});
function loadData() {
    dataTable = $("#tblCompanyData").DataTable({
        "ajax": {
                    "url": "/Admin/Company/GetAllCompany"
                },
        "columns": [
            { "data": "name", "width":"15%"},
            { "data": "streetAddress", "width": "15%"},
            { "data": "city", "width": "15%"},
            { "data": "state", "width": "15%"},
            { "data": "postalCode", "width": "15%"},
            { "data": "phoneNumber", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                         <div class="w-75 btn-group" role="group">
                           <a class="btn btn-primary" href="/Admin/Company/Upsert/${data}">
                                        <i class="bi bi-pencil-square"></i> Edit
                           </a>
                           &nbsp;
                           <a class="btn btn-danger" onClick="Delete('/Admin/Company/Delete/${data}')">
                                        <i class="bi bi-trash"></i> Delete
                           </a>
                        </div>
                    `
                },
                "width" : "30%"
            }
        ]
    });
}
function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                method: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}