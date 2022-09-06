$(function () {

    //刪除
    $(".deleteProduct").on("click", function (e) {
        var isDelete = confirm("是否要刪除此商品？");

        if (!isDelete) {
            return ;
        }

        var deletedProductId = e.target.id;
        var deletedRow = e.target.closest("tr");

        $.post("/Product/Delete", { productId: deletedProductId })
            .done(function () {
                deletedRow.remove();
                alert("刪除成功！");
            })
            .fail(function () {
                alert("刪除失敗！");
            })
    });

    //新增確認
    $("#addProductForm").submit(function (e) {
        if (!confirm("是否要新增此商品？")) {
            e.preventDefault();
        }
    });

    //更新確認
    $("#updateProductForm").submit(function (e) {
        if (!confirm("是否要更新此商品資料？")) {
            e.preventDefault();
        }
    });
});