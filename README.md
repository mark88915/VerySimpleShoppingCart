## 動機
銜接期不知道要寫什麼就先隨便寫寫(連Naming都很醜的那種)

拿來複習 .Net Framework MVC

還有學習使用客製Authorize Filter搭配Session來做登入驗證

## 使用到的技術
前端：Razor、jQuery、Ajax

後端：C#(.Net Framework MVC)

DataBase：Sql Server

## Files
VerySimpleShoppingCart

├─shoppingCart.sln

└─shoppingCart

    ├─Controllers

    │   ├─ProductController.cs // Admin端Controller

    │   ├─LoginController.cs // 選擇登入哪端的Controller

    │   └─ClientController.cs // 客戶端Controller

    │

    ├─Models

    │   ├─Client

    │   │   ├─User.cs // 帳戶Model

    │   │   ├─UserPurchase.cs // 購買物品Model

    │   │   ├─PurchaseHistory.cs // 購買歷史Model
    
    │   │   ├─UserService // 與使用者帳戶相關之商業邏輯

    │   │   └─UserProductService.cs // 客戶端購買商品之商業邏輯

    │   ├─Filter

    │   │   └─CustomAuthorizeAttribute.cs // 客製Authorize Filter

    │   ├─Product

    │   │   ├─Product.cs // Admin端商品Model

    │   │   └─ProductService.cs // Admin端商品相關之商業邏輯

    │   └─CommonTool.cs // 取得連線字串用的工具

    │
    ├─Scripts

    │   ├─Client

    │   │   ├─Client.js // 客戶端前端處理

    │   └─Product

    │       └─Product.js // Admin端前端處理

    │
    └─Views

        ├─Client

        │   ├─LoginPage.cshtml // 登入畫面

        │   ├─ProductList.cshtml // 商品清單(首頁)

        │   ├─Purchase.cshtml // 商品購買畫面

        │   ├─PurchaseHistory.cshtml // 購買歷史畫面

        │   ├─Register.cshtml // 帳號註冊畫面

        │   └─ShoppingCart.cshtml // 購物車畫面

        ├─Login

        │   └─Index.cshtml // 選擇哪端登入的畫面

        └─Product

            ├─Index.cshtml // Admin端首頁

            ├─Create.cshtml // 新增商品畫面
            
            └─Update.cshtml // 更新商品資訊畫面