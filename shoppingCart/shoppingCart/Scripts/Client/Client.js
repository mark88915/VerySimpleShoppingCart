$(function () {
    $(".AddProductToCart").on("click", function (e) {
        var productId = e.target.id;

        $.ajax({
            url: "/Client/AddProductToShoppingCart",
            type: "POST",
            data: { productId: productId }
        })
            .done(function (response) {
                if (response.IsAddToShoppingCartSuccess) {
                    alert("成功將商品加入購物車！");
                } else if (response.ThisProductExistInShoppingCart) {
                    alert("此商品已存在購物車中！");
                }
            })
            .fail(function () {
                alert("系統發生錯誤，未將商品加入購物車！");
            });
    });

    $("#LogoutForm").submit(function (e) {
        if (!confirm("是否確定要登出？")) {
            e.preventDefault();
        }
    });

    $(".RemoveFromShoppingCart").on("click", function (e) {
        var productId = e.target.id;

        if (confirm("是否將此商品移出購物車？")) {
            $.ajax({
                url: "/Client/RemoveFromShoppingCart",
                type: "POST",
                data: { productId: productId }
            })
                .done(function (response) {
                    if (response.IsRemoveSuccess) {
                        e.target.closest("tr").remove();
                        alert("移出購物車成功");
                    } else {
                        alert("移出購物車失敗");
                    }
                })
                .fail(function () {
                    alert("系統發生錯誤，未將商品移出購物車");
                });
        }
    });
});