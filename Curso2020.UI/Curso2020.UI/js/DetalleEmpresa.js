$(document).ready(function () {
    function getParameterByName(name) {
        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
            results = regex.exec(location.search);
        return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    }

    const generarTablaDe = (data) => {

        let html = "<table class=\"table\"><thead><tr><th>Nombre</th><th>Cuit</th><th>Direccion</th><th>Razon Social</th></tr></thead><tbody>";

        const getFilaCon = (Nombre, Cuit, Direccion, RazonSocial) => {
            return "<tr class=\"table - light\"><td>" + Nombre + " </td><td>" + Cuit + " </td><td>" + Direccion + " </td><td>" + RazonSocial + "</td></tr>";
        };
        html += getFilaCon(data.nombre, data.cuit, data.direccion, data.razonSocial);

        html += "</tbody></table>";
        return html;

    };

    var cuit = getParameterByName('cuit');

    $.ajax({
        url: "https://localhost:5001/api/Empresa/Detail?cuit=" + cuit,
        method: "POST",
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
            //aca llamar alguna funcion para mostrar la empresa
            var html = generarTablaDe(data);

            $("#tabla").append(html);
        },

    });
    $("#btnDelete").click(() => {
        var decision = confirm("Desea borrar la empresa?");
        if (decision) {
            $.ajax({
                url: "https://localhost:5001/api/Empresa/Delete",
                method: "POST",
                crossDomain: true,
                headers: {
                    "Accept": "application/json",
                },
                data: cuit,
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                success: function () {
                    location.reload();
                },
                error: function (xhr) {
                    alert(xhr.message);
                    location.reload();
                }
            });
        }
    });

    $("#modifyOk").click(() => {
        let nombre = $("#txtnombre").val();
        let razonsocial = $("#txtrazonsocial").val();
        let direccion = $("#txtdireccion").val();
        if (cuit.length == 11 && (nombre.length == 0 || nombre.length > 2) && (razonsocial.length == 0 || razonsocial.length > 2) && (direccion.length == 0 || direccion.length > 2)) {
            let data = { Cuit: cuit, Nombre: nombre, RazonSocial: razonsocial, Direccion: direccion };
            $.ajax({
                url: "https://localhost:5001/api/Empresa/Modificar",
                method: "POST",
                crossDomain: true,
                headers: {
                    "Accept": "application/json",
                },
                data: JSON.stringify(data),
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    alert(data.message);
                    location.reload();
                },
                error: function (xhr) {
                    alert(data.message);
                    location.reload();
                }
            });
        } else {
            alert("complete los campos correctamente");
        };
    });
    $("#btnModificar").click(() => {

        $("#modificarEmpresa").show();
    })
});
