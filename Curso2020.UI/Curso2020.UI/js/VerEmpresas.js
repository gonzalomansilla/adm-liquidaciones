$(document).ready(function () {
    const generarTablaDe = (data) => {
        debugger;
        let html = "<table class=\"table\"><thead><tr><th>Nombre</th><th>Cuit</th></tr></thead><tbody>";

        const getFilaCon = (Nombre, Cuit) => {
            var str = "<a href=" + "DetalleEmpresa.html?cuit=" + Cuit + ">";
            return "<tr class=\"table - light\"><td>" + Nombre + " </td><td>" + str + Cuit + "</a></td></tr>";
        };


        for (let p of data) {
            html += getFilaCon(p.nombre, p.cuit);
        }

        html += "</tbody></table>";
        return html;

    };

    $.ajax({
        url: "https://localhost:5001/api/Empresa/Grid",
        method: "GET",
        crossDomain: true,
        headers: {
            "Accept": "application/json",
        },

        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (typeof data !== "object") {
                data = data.d || data;
                data = JSON.parse(data);
            }
            var html = generarTablaDe(data);

            $("#tabla").append(html);

        },
        //error: function (xhr) {
        //    debugger;
        //    let response = JSON.parse(xhr.responseText);
        //    switch (xhr.status) {
        //        case 400:
        //        case 401:
        //            alert(response.message)
        //            break;
        //        default:
        //            alert(JSON.stringify(xhr));
        //    }
        //}
    });

    $("#altaOk").click(function () {
        let cuit = ($("#cuitEmpresa").val());
        let nombreE = $("#nombreEmpresa").val();
        let razonSocial = $("#razonSocial").val();
        let direccion = $("#direccionEmpresa").val();
        let data = { Cuit: cuit, Nombre: nombreE, RazonSocial: razonSocial, Direccion: direccion };
        $.ajax({
            url: "https://localhost:5001/api/Empresa/Alta",
            method: "POST",
            crossDomain: true,
            headers: {
                "Accept": "application/json",
            },
            data: JSON.stringify(data),
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (typeof data !== "object") {
                    data = data.d || data;
                    data = JSON.parse(data);
                }
                let message = data.message || data.Message;
                alert(message);

                window.location = "/EmpresaUI/VerEmpresas.html";
            },
            error: function (xhr) {
                debugger;
                let response = JSON.parse(xhr.responseText);
                switch (xhr.status) {
                    case 400:
                        alert(response.message);
                    case 401:
                        
                        break;
                    default:
                        alert(JSON.stringify(xhr));
                }
            }
        });
    });

    $("#altaEmpresa").click(() => {

        $("#containerEmpresa").show();
    });

});