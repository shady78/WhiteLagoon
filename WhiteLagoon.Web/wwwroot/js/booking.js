var dataTable;

$(document).ready(function () {
    const urlParams = new URLSearchParams(window.location.search);
    const status = urlParams.get('status');
    loadTable(status);
});

function loadTable(status) {
    dataTable = $('#tblBookings').DataTable({

        "ajax": {
            url: '/booking/getall?status=' + status
        },
        "columns": [
            { data: 'id', "width": "10%" },
            { data: 'name', "width": "10%" },
            { data: 'phone', "width": "10%" },
            { data: 'email', "width": "10%" },
            { data: 'status', "width": "10%" },
            { data: 'checkInDate', "width": "10%" },
            { data: 'nights', "width": "10%" },
            { data: 'totalCost', render: $.fn.dataTable.render.number(',', '.', 2), "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group">
                        <a class="btn btn-outline-warning mx-2" href="/booking/bookingDetails?bookingId=${data}">
                            <i class="bi bi-pencil-square"></i> Details
                        </a>
                    </div>`
                }
            }

        ]
    });
}