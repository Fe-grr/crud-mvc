function Add() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var categoriaObj = {
        CategoriaId: $('#CategoriaId').val(),
        Nome: $('#Nome').val(),
    };
    $.ajax({
        url: "Categorias/Create",
        data: JSON.stringify(categoriaObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#myModal').modal('hide');

            if (!result.sucesso) {
                toastr["error"](result.msg);
            }

            else {
                toastr["success"](result.msg);
                setInterval('location.reload()', 1200);
            }
        },
        error: function (erro) {
            toastr["error"](erro.responseText);
        }
    });
}

function getbyID(CategoriaId) {
    $('#Nome').css('border-color', 'lightgrey');
    
    $.ajax({
        url: "Categorias/Edit?ID=" + CategoriaId,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#myModalLabel')[0].innerText = "Editar Produto";
            $('#CategoriaId').val(result.CategoriaId);
            $('#Nome').val(result.Nome);

            createComboBox();
            $('#myModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();
        },
        error: function (erro) {
            toastr["error"](erro.responseText);
        }
    });
    return false;
}

function Update() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var categoriaObj = {
        CategoriaId: $('#CategoriaId').val(),
        Nome: $('#Nome').val(),
       
        
        
    };
    $.ajax({
        url: "Categorias/Edit",
        data: JSON.stringify(categoriaObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (!result.sucesso) {
                toastr["error"](result.msg);
            }
            else {
                $('#myModal').modal('hide');
                $('#CategoriaId').val("");
                $('#Nome').val("");
                $('#Preco').val("");
                $('#Descricao').val("");
                $('#Country').val("");
                toastr["success"](result.msg);
                setInterval('location.reload()', 1200);
            }
        },
        error: function (erro) {
            toastr["error"](erro.responseText);
        }
    });
}

function Delete(ID) {
    var ans = confirm("Tem certeza que gostaria de excluir este produto?");
    if (ans) {
        $.ajax({
            url: "Categorias/Delete?ID=" + ID,
            type: "POST",
            cache: false,
            dataType: "json",
            success: function (result) {
                if (!result.sucesso) {
                    toastr["error"](result.msg);
                }

                else {
                    toastr["success"](result.msg);
                    setInterval('location.reload()', 1200);
                }
            },
            error: function (erro) {
                toastr["error"](erro.responseText);
            }
        });
    }
}
function preparaModel() {
    this.clearTextBox();
    this.createComboBox();
}

function createComboBox() {
    $.ajax({
        url: "/Categorias/Get/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $.each(result, function (id, categoria) {
                var option = new Option(categoria.Nome, categoria.CategoriaId);
                $(option).html(categoria.Nome);
                $("#Categoria").append(option);
            })
        },
        error: function (erro) {
            alert(erro.responseText);
        }
    });
}

function clearTextBox() {
    $('#CategoriaId').val("");
    $('#Nome').val("");
    $('#Preco').val("");
    $('#Descricao').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#Nome').css('border-color', 'lightgrey');
    
    
}

function validate() {
    var isValid = true;
    if ($('#Nome').val().trim() == "") {
        $('#Nome').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Nome').css('border-color', 'lightgrey');
    }
    return isValid;
}