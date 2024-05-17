// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('.date').mask('00/00/0000');
    $('#cep').mask('00000-000');
    $('#phone').mask('0000-0000');
    $('#phone_with_ddd').mask('(00)0000-0000');
    $('.mixed').mask('AAA 000-S0S');
    $('#cpf').mask('000.000.000-00', { reverse: true });
    $('#cnpj').mask('00.000.000/0000-00', { reverse: true });
    $('#money2').mask("#.##0,00", { reverse: true });
    $('.money').mask('000.000.000.000.000,00', { reverse: true });
});