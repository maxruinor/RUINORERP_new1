using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteRules.aliexpress
{
    /// <summary>
    /// 保留
    /// </summary>
    public class SkuPropertyValues
    {
        /// <summary>
        /// 
        /// </summary>
        public string propertyValueDefinitionName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string propertyValueDisplayName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string propertyValueId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string propertyValueIdLong { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string propertyValueName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string skuColorValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string skuPropertyImagePath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string skuPropertyImageSummPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string skuPropertyTips { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string skuPropertyValueShowOrder { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string skuPropertyValueTips { get; set; }
    }

    /// <summary>
    /// 保留
    /// </summary>
    public class ProductSKUPropertyList
    {
        /// <summary>
        /// 
        /// </summary>
        public bool isShowTypeColor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string order { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string showType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool showTypeColor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string skuPropertyId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string skuPropertyName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SkuPropertyValues> skuPropertyValues { get; set; }
    }

    public class SkuActivityAmount
    {
        /// <summary>
        /// 
        /// </summary>
        public string currency { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string formatedAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double value { get; set; }
    }

    /// <summary>
    /// 保留
    /// </summary>
    public class SkuAmount
    {
        /// <summary>
        /// 
        /// </summary>
        public string currency { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string formatedAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double value { get; set; }
    }

    /// <summary>
    /// 保留
    /// </summary>
    public class SkuVal
    {
        /// <summary>
        /// 
        /// </summary>
        public string actSkuCalPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string actSkuMultiCurrencyCalPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string actSkuMultiCurrencyDisplayPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string availQuantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string inventory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool isActivity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> optionalWarrantyPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SkuActivityAmount skuActivityAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SkuAmount skuAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string skuCalPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string skuMultiCurrencyCalPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string skuMultiCurrencyDisplayPrice { get; set; }
    }



    /// <summary>
    /// 保留
    /// </summary>
    public class SkuPriceList
    {
        /// <summary>
        /// 
        /// </summary>
        public string freightExt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string skuAttr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string skuId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string skuIdStr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string skuPropIds { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SkuVal skuVal { get; set; }
    }

    /// <summary>
    /// skuModule 节点
    /// {"actionModule":{"addToCartUrl":"//shoppingcart.aliexpress.com/addToShopcart4Js.htm","aeOrderFrom":"main_detail","allowVisitorAddCart":true,"cartDetailUrl":"//shoppingcart.aliexpress.com/shopcart/shopcartDetail.htm","categoryId":200001565,"companyId":110097067,"confirmOrderUrl":"//shoppingcart.aliexpress.com/order/confirm_order.htm","features":{},"freightExt":"{\"p1\":\"8.70\",\"p2\":\"18.89\",\"p3\":\"USD\",\"p4\":\"990000\",\"p5\":\"0\"}","i18nMap":{"WISH_MAX_NOTICE":"Oops, it seemed you already reached the list’s maximum.","BUYER_ABLE":"This feature is only available to buyers.","WISHLIST_SAVE_BUTTON":"Save","WISH_MOVE_TO_ANOTHER_LIST_TIPS":"Move to another list","ADD_X_MORE":"Congratulations! You've earned <div class=\"before-coin-count\"></div>{coinCount} coins. You have {number} chances today to continue to earn coins.","SC_ADD_SUCC":"A new item has been added to your Shopping Cart.","WISHLIST_PUBLIC_NOTICE":"(Anyone can see this list and share it)","WISH_DETAULT_LIST":"Wish List","WISH_CREATE_LIST":"Create a Wish List","WL_ERROR":"Network is busy, please try again","WISH_NAME_ALREADY_USE":"This Wish List name is already in use.","WISH_REVISELIST":"Edit Your List","SC_ADD_FAILED":"Failed to add, please refresh the page and re-click the \"Add to Cart\"","SC_HAVE":"You now have {{number}} items in your Shopping Cart.","ADD_TO_CART":"Add to Cart","WISH_CANCEL_WISHLIST":"Cancel","BUY_NOW":"Buy Now","WISH_SYSTEM_ERROR":"Oops, system error. Please try again.","SC_ADD_MAX":"You can add {{number}} items to cart at the most. Please remove some items before adding more.","SC_VIEW":"View Shopping Cart","WISH_YOUMAYLIKE":"You may also like","WISH_VIEW_WISH_LIST":"View Wish List","SC_RECOMMEND":"Buyers Who Bought This Item Also Bought:","CONTINUE":"Continue Shopping","ADDED_TO":"Added to {wishlist} ","ORDER_NOW":"Order now","SELECT_TIP":"Please provide the missing information first","NO_LONGER_AVAILABLE":"Sorry, this item is no longer available!","PREVIEW_PERIOD":"2019/3/21 00:00:00 GMT-0700,2019/3/27 23:59:59 GMT-0700","WISH_MAX_GROUP":"Sorry, you can't create more wish lists","WISHLIST_PUBLIC_TIP":"Public"},"id":0,"itemStatus":0,"itemWished":false,"itemWishedCount":305,"localSeller":false,"name":"ActionModule","preSale":false,"productId":1739481085,"rootCategoryId":13,"showCoinAnim":false,"showShareHeader":true,"storeNum":408908,"totalAvailQuantity":8548},"aePlusModule":{"features":{},"i18nMap":{},"id":0,"name":"AePlusModule"},"buyerProtectionModule":{"buyerProtection":{"brief":"Money back guarantee","detailDescription":"","id":2,"name":"{day}-Day Buyer Protection","redirectUrl":"https://sale.aliexpress.com/v8Yr8f629D.htm","type":1},"features":{},"i18nMap":{"PLAZA_BUYER_PROTECTION_TITLE":"Local Return","PLAZA_BUYER_PROTECTION_TIP":"Dispones de 15 días en los que puedes devolver el artículo por no satisfacer tus expectativas, siempre que se encuentre en perfecto estado, sin usar y conserve todas las etiquetas y el embalaje original","PLAZA_BUYER_PROTECTION_CONTENT":"15 days"},"id":0,"name":"BuyerProtectionModule"},"commonModule":{"activity":true,"buyerAccountId":240193246,"buyerAdminSeq":240193246,"categoryId":200001565,"currencyCode":"USD","features":{},"gagaDataSite":"en","i18nMap":{},"id":0,"name":"CommonModule","plaza":false,"preSale":false,"productId":1739481085,"productTags":{},"reminds":[],"sellerAdminSeq":118739230,"tradeCurrencyCode":"USD","userCountryCode":"CN","userCountryName":"China (Mainland)"},"couponModule":{"buyerAdminSeq":240193246,"currencyCode":"USD","features":{},"i18nMap":{"GET_COUPONS":"Get coupons","SCP_ERR_HAVE":"Sorry, you have already got the coupon from this store! Use coupon now!","OFF_ON":"{money1} off on {money2}","ORDER_OVER":"Orders over ","code52Title":"Sorry, there are no more coupons available.","GET_IT":"Get Now","GET_NOW":"Get Now","GET_MORE":"Get more","systemError":"Oops, something went wrong. Please try again.","OFF_PER":"{money1} off per {money2}","STORE_COUPON":"Store Coupon","SHOP_COUPONE_TIME_START_END":"From {0} to {1}","NEW_USER_COUPON_ACQUIRE_FAIL_ALREADY_HAVE":"Sorry, you already have a New User Coupon.","code50Title":"Sorry, the coupon could not be issued. Please try again.","NEW_USER_COUPON_ACQUIRE_FAIL_NOT_AVAILABLE_NOW":"Sorry, New User Coupons are not available now.","NEW_USER_COUPON_ACQUIRE_FAIL_GROUP_LIMIT":"Sorry, you have the maximum amount of New User Coupons.","code14Title":"You've already collected these Select Coupons.","NEW_USER_COUPON_ACQUIRE_FAIL_NOT_NEW_USER":"Sorry, only new users are eligible to get this coupon.","SHOP_PROMO_CODE_COPIED":"Promo code copied!","ADDED":"Added","NEW_USER_COUPON_ACQUIRE_FAIL_SECURITY":"To protect the security of your account, please use another device to sign in.","SHOP_PROMO_CODE_TITLE":"Store promo code","NEW_USER_COUPON_ACQUIRE_FAIL":"Oops, something went wrong! Please try again.","NEW_USER_COUPON_ACQUIRE_FAIL_LIMIT_DAY":"Sorry, New User Coupons are not available today.","NEW_USER_COUPON_ACQUIRE_FAIL_REGISTER_COUNTRY_LIMIT":"Please check that this coupon is redeemable in your registered country.","SCP_ERR_NONE":"Sorry! All of these coupons have been allocated.","NEW_USER_COUPON_ACQUIRE_FAIL_GRANT_ERROR":"Sorry, you failed to get this coupon.","code17Title":"Sorry, this coupon is no longer available.","SHOP_COUPONE_TIME_EXPIRES":"Expires {0}","SHOPPONG_CREDIT":"SHOPPONG CREDIT","EXCHANGE_MORE":"Exchange more","NEW_USER_COUPON_ACQUIRE_FAIL_SYSTEM_ERROR":"Oops, something went wrong! Please try again.","MY_BALANCE":"Balance","INSTANT_DISCOUNT":"Instant discount","EXCHANGE_NOW":"Exchange now","CROSS_STORE_VOUCHER_TIP":"Save up to {money1} on orders over {money2}","NEW_USER_COUPON":"New User Coupon","GET":"GET","SHOP_PROMO_CODE_COP_FAILED":"Failed to copy. Please try again","code-30005Title":"Uh, oh… It looks like you don't have enough coins yet.","MY_COINS":"My Coins ","BUY_GET_OFF":"Buy {fullpiece} get {fulldiscount} off","code51Title":"Sorry, there are no more coupons available.","NEW_USER_COUPON_ACQUIRE_FAIL_LIMIT_HOUR":"Sorry, New User Coupons are not available at this hour.","CROSS_STORE_VOUCHER":"Select coupon "},"id":0,"name":"CouponModule"},"crossLinkModule":{"breadCrumbPathList":[{"cateId":0,"name":"Home","remark":"","url":"//www.aliexpress.com/"},{"cateId":0,"name":"All Categories","remark":"","target":"All Categories","url":"//www.aliexpress.com/all-wholesale-products.html"},{"cateId":39,"name":"Lights & Lighting","remark":"","target":"Lights & Lighting","url":"//www.aliexpress.com/category/39/lights-lighting.html"},{"cateId":530,"name":"Lighting Accessories","remark":"","target":"Lighting Accessories","url":"//www.aliexpress.com/category/530/lighting-accessories.html"},{"cateId":200003079,"name":"Connectors","remark":"","target":"Connectors","url":"//www.aliexpress.com/category/200003079/connectors.html"}],"crossLinkGroupList":[{"channel":"Related Search","crossLinkList":[{"displayName":"keyboard lenovo t420","name":"keyboard lenovo t420","url":"https://www.aliexpress.com/popular/keyboard-lenovo-t420.html"},{"displayName":"5 pin audio connector","name":"5 pin audio connector","url":"https://www.aliexpress.com/popular/5-pin-audio-connector.html"},{"displayName":"xlr 3 7 pin","name":"xlr 3 7 pin","url":"https://www.aliexpress.com/w/wholesale-xlr-3-7-pin.html"},{"displayName":"socket xlr","name":"socket xlr","url":"https://www.aliexpress.com/w/wholesale-socket-xlr.html"},{"displayName":"xlr conector ne","name":"xlr conector ne","url":"https://www.aliexpress.com/w/wholesale-xlr-conector-ne.html"},{"displayName":"gx16","name":"gx16","url":"https://www.aliexpress.com/w/wholesale-gx16.html"},{"displayName":"eyewear titanium","name":"eyewear titanium","url":"https://www.aliexpress.com/popular/eyewear-titanium.html"},{"displayName":"plug xlr","name":"plug xlr","url":"https://www.aliexpress.com/w/wholesale-plug-xlr.html"},{"displayName":"gx16 9 pin","name":"gx16 9 pin","url":"https://www.aliexpress.com/w/wholesale-gx16-9-pin.html"},{"displayName":"5 pin xlr","name":"5 pin xlr","url":"https://www.aliexpress.com/w/wholesale-5-pin-xlr.html"},{"displayName":"mini xlr 4 pin fem","name":"mini xlr 4 pin fem","url":"https://www.aliexpress.com/popular/mini-xlr-4-pin-fem.html"},{"displayName":"mini xlr","name":"mini xlr","url":"https://www.aliexpress.com/w/wholesale-mini-xlr.html"}],"name":"Related Search"},{"channel":"Hot Search","crossLinkList":[{"displayName":"ropa perros monos","name":"ropa perros monos","url":"https://www.aliexpress.com/popular/ropa-perros-monos.html"},{"displayName":"red elastica reti","name":"red elastica reti","url":"https://www.aliexpress.com/popular/red-elastica-reti.html"},{"displayName":"rollerblade walze","name":"rollerblade walze","url":"https://www.aliexpress.com/popular/rollerblade-walze.html"},{"displayName":"rebajas camisetas","name":"rebajas camisetas","url":"https://www.aliexpress.com/popular/rebajas-camisetas.html"},{"displayName":"salomón chaquetas","name":"salomón chaquetas","url":"https://www.aliexpress.com/popular/salom%C3%B3n-chaquetas.html"},{"displayName":"wolksvagen rokken","name":"wolksvagen rokken","url":"https://www.aliexpress.com/popular/wolksvagen-rokken.html"},{"displayName":"scrapbox stickers","name":"scrapbox stickers","url":"https://www.aliexpress.com/popular/rank_scrapbox-stickers.html"},{"displayName":"saturno v blocchi","name":"saturno v blocchi","url":"https://www.aliexpress.com/popular/saturno-v-blocchi.html"},{"displayName":"shapewear shapers","name":"shapewear shapers","url":"https://www.aliexpress.com/popular/rank_shapewear-shapers.html"}],"name":"Hot Search"}],"features":{},"i18nMap":{"BREADCRUMB_PART2":"and you can find similar products at","BREADCRUMB_PART1":"This product belongs to","CROSSLINK_RELATED_SEARCHES":"Links"},"id":0,"name":"CrossLinkModule","productId":1739481085},"descriptionModule":{"descriptionUrl":"https://aeproductsourcesite.alicdn.com/product/description/pc/v2/en_US/desc.htm?productId=1739481085&key=HTB1TfEYX5jrK1RjSsplM7NHmVXa5.zip&token=b0d12f7faa75f887813404c61d3f202d","features":{},"i18nMap":{},"id":0,"name":"DescriptionModule","productId":1739481085,"sellerAdminSeq":118739230},"features":{},"feedbackModule":{"companyId":110097067,"features":{},"feedbackServer":"//feedback.aliexpress.com","i18nMap":{},"id":0,"name":"FeedbackModule","productId":1739481085,"sellerAdminSeq":118739230,"trialReviewNum":0},"groupShareModule":{"features":{},"i18nMap":{},"id":0,"name":"GroupShareModule"},"i18nMap":{"VIEW_MORE":"View More","ASK_BUYERS":"Buyer Questions & Answers","PAGE_NOT_FOUND_NOTICE":"Sorry, this page is unavailable, but check out our other pages that are just as great. ","VIEW_5_MORE_ANSWERS":"View More","openTrace":"true","PAGE_NOT_FOUND_RCMD_TITLE":"Sorry, the page you requested can not be found:("},"imageModule":{"features":{},"i18nMap":{},"id":0,"imagePathList":["https://ae01.alicdn.com/kf/HTB1atMgQpXXXXXzXXXXq6xXFXXX0/10-set-2-3-4-5-6-7-8-9-10-pin-aviation-connector-XLR-Audio.jpg","https://ae01.alicdn.com/kf/HTB1Wkc1XZrrK1Rjy1zeq6xalFXat/10-set-2-3-4-5-6-7-8-9-10-pin-aviation-connector-XLR-Audio.jpg","https://ae01.alicdn.com/kf/HTB1tn.RXVzsK1Rjy1Xbq6xOaFXaP/10-set-2-3-4-5-6-7-8-9-10-pin-aviation-connector-XLR-Audio.jpg","https://ae01.alicdn.com/kf/HTB1iZlnIXXXXXcVXFXXq6xXFXXXS/10-set-2-3-4-5-6-7-8-9-10-pin-aviation-connector-XLR-Audio.jpg","https://ae01.alicdn.com/kf/HTB1vkVtIXXXXXbdXFXXq6xXFXXXu/10-set-2-3-4-5-6-7-8-9-10-pin-aviation-connector-XLR-Audio.jpg","https://ae01.alicdn.com/kf/HTB1fNhzIXXXXXaQXpXXq6xXFXXXv/10-set-2-3-4-5-6-7-8-9-10-pin-aviation-connector-XLR-Audio.jpg"],"name":"ImageModule","summImagePathList":["https://ae01.alicdn.com/kf/HTB1atMgQpXXXXXzXXXXq6xXFXXX0/10-set-2-3-4-5-6-7-8-9-10-pin-aviation-connector-XLR-Audio.jpg_50x50.jpg","https://ae01.alicdn.com/kf/HTB1Wkc1XZrrK1Rjy1zeq6xalFXat/10-set-2-3-4-5-6-7-8-9-10-pin-aviation-connector-XLR-Audio.jpg_50x50.jpg","https://ae01.alicdn.com/kf/HTB1tn.RXVzsK1Rjy1Xbq6xOaFXaP/10-set-2-3-4-5-6-7-8-9-10-pin-aviation-connector-XLR-Audio.jpg_50x50.jpg","https://ae01.alicdn.com/kf/HTB1iZlnIXXXXXcVXFXXq6xXFXXXS/10-set-2-3-4-5-6-7-8-9-10-pin-aviation-connector-XLR-Audio.jpg_50x50.jpg","https://ae01.alicdn.com/kf/HTB1vkVtIXXXXXbdXFXXq6xXFXXXu/10-set-2-3-4-5-6-7-8-9-10-pin-aviation-connector-XLR-Audio.jpg_50x50.jpg","https://ae01.alicdn.com/kf/HTB1fNhzIXXXXXaQXpXXq6xXFXXXv/10-set-2-3-4-5-6-7-8-9-10-pin-aviation-connector-XLR-Audio.jpg_50x50.jpg"]},"installmentModule":{"features":{},"i18nMap":{},"id":0,"name":"InstallmentModule"},"middleBannerModule":{"features":{},"i18nMap":{"END_IN":"Ends in","DAYS":"{number} days","DAY":"{number} day","STARTS_IN":"Starts in"},"id":0,"name":"MiddleBannerModule","showUniformBanner":false},"name":"ItemDetailResp","otherServiceModule":{"features":{},"hasWarrantyInfo":false,"i18nMap":{"TAB_SPECS":"Specifications","PLAZA_SERVICE_SUBTITLE_PC":"Guarantee in Spain","PLAZA_SERVICE_WARRANTY_EMAIL":"Email","PLAZA_SERVICE_WARRANTY_PHONE":"Phone","PLAZA_SERVICE_WARRANTY_HOURS":"Hours","TAB_CUSTOMER_REVIEWS":"Customer Reviews","PLAZA_SERVICE_WARRANTY_WEBSITE":"Website","TAB_OVERVIEW":"Overview","PLAZA_SERVICE_WARRANTY_BRAND":"Brand","PLAZA_SERVICE_WARRANTY_CATEGORY":"Category","PLAZA_SERVICE_TITLE_PC":"Plaza Technology Guarantees","PLAZA_SERVICE_CONTENT3_3_PC":"- The safety seal is not damaged and all labels are retained.","PLAZA_SERVICE_WARRANTY_TITLE":"Official technical service","TAB_REPORT_ITEM":"Report Item","TAB_SERVICE":"Service","PLAZA_SERVICE_CONTENT3_1_PC":"You have 15 days to return your Plaza Technology order, provided that:","PLAZA_SERVICE_CONTENT3_2_PC":"- It is in perfect condition and in the original packaging. ","PLAZA_SERVICE_CONTENT1_PC":"All items of Plaza Technology are 100% original, are covered by the protection of the buyer of AliExpress and have an official warranty of 2 years in Spain to process in the official technical service in Spain designated by the manufacturer.","PLAZA_SERVICE_SUBTITLE2_PC":"Shipping and delivery","PLAZA_SERVICE_CONTENT2_PC":"Shipments are free without minimum purchase. We make all our shipments from Spain, so there are no additional fees or customs. The delivery time in any point of the peninsula is 1 to 3 working days from the moment of purchase. At the moment we do not send to the Canary Islands, Ceuta or Melilla.","PLAZA_SERVICE_SUBTITLE3_PC":"Returns"},"id":0,"name":"OtherServiceModule"},"pageModule":{"aeOrderFrom":"main_detail","aerSelfOperation":false,"amphtmlTag":"<meta>","boutiqueSeller":false,"canonicalTag":"<link rel=\"canonical\" href=\"https://www.aliexpress.com/item/1739481085.html\">","complaintUrl":"//report.aliexpress.com/health/reportIndex.htm?product_id=1739481085&b_login_id=cn118813292","description":"Cheap Connectors, Buy Quality Lights & Lighting Directly from China Suppliers:10 set  2/3/4/5/6/7/8/9/10 pin aviation  connector XLR Audio Cable Connector male and female  Panel Chassis Mount Kit  16mm\nEnjoy ✓Free Shipping Worldwide! ✓Limited Time Sale ✓Easy Return.","features":{},"hreflangTag":"<link rel=\"alternate\" hreflang=\"en\" href=\"https://www.aliexpress.com/item/1739481085.html\"/>\n<link rel=\"alternate\" hreflang=\"id\" href=\"https://id.aliexpress.com/item/1739481085.html\"/>\n<link rel=\"alternate\" hreflang=\"ko\" href=\"https://ko.aliexpress.com/item/1739481085.html\"/>\n<link rel=\"alternate\" hreflang=\"ar\" href=\"https://ar.aliexpress.com/item/1739481085.html\"/>\n<link rel=\"alternate\" hreflang=\"de\" href=\"https://de.aliexpress.com/item/1739481085.html\"/>\n<link rel=\"alternate\" hreflang=\"es\" href=\"https://es.aliexpress.com/item/1739481085.html\"/>\n<link rel=\"alternate\" hreflang=\"fr\" href=\"https://fr.aliexpress.com/item/1739481085.html\"/>\n<link rel=\"alternate\" hreflang=\"it\" href=\"https://it.aliexpress.com/item/1739481085.html\"/>\n<link rel=\"alternate\" hreflang=\"nl\" href=\"https://nl.aliexpress.com/item/1739481085.html\"/>\n<link rel=\"alternate\" hreflang=\"pt\" href=\"https://pt.aliexpress.com/item/1739481085.html\"/>\n<link rel=\"alternate\" hreflang=\"th\" href=\"https://th.aliexpress.com/item/1739481085.html\"/>\n<link rel=\"alternate\" hreflang=\"tr\" href=\"https://tr.aliexpress.com/item/1739481085.html\"/>\n<link rel=\"alternate\" hreflang=\"vi\" href=\"https://vi.aliexpress.com/item/1739481085.html\"/>\n<link rel=\"alternate\" hreflang=\"he\" href=\"https://he.aliexpress.com/item/1739481085.html\"/>\n<link rel=\"alternate\" hreflang=\"ru\" href=\"https://aliexpress.ru/item/1739481085.html\"/>\n<link rel=\"alternate\" hreflang=\"ja\" href=\"https://ja.aliexpress.com/item/1739481085.html\"/>\n<link rel=\"alternate\" hreflang=\"pl\" href=\"https://pl.aliexpress.com/item/1739481085.html\"/>","i18nMap":{},"id":0,"imagePath":"https://ae01.alicdn.com/kf/HTB1atMgQpXXXXXzXXXXq6xXFXXX0/10-set-2-3-4-5-6-7-8-9-10-pin-aviation-connector-XLR-Audio.jpg","itemDetailUrl":"https://www.aliexpress.com/item/1739481085.html","keywords":"Connectors,Lights & Lighting,Cheap Connectors,High Quality Lights & Lighting,connector male,connectors xlr,connector set","kidBaby":false,"mSiteUrl":"https://m.aliexpress.com/item/1739481085.html","mediaTag":"<link rel=\"alternate\" media=\"only screen and (max-width: 640px)\" href=\"https://m.aliexpress.com/item/1739481085.html\"/>","multiLanguageUrlList":[{"language":"msite","languageUrl":"https://m.aliexpress.com/item/1739481085.html"},{"language":"en","languageUrl":"https://www.aliexpress.com/item/1739481085.html"},{"language":"it","languageUrl":"https://it.aliexpress.com/item/1739481085.html"},{"language":"fr","languageUrl":"https://fr.aliexpress.com/item/1739481085.html"},{"language":"de","languageUrl":"https://de.aliexpress.com/item/1739481085.html"},{"language":"ru","languageUrl":"https://aliexpress.ru/item/1739481085.html"},{"language":"es","languageUrl":"https://es.aliexpress.com/item/1739481085.html"},{"language":"pt","languageUrl":"https://pt.aliexpress.com/item/1739481085.html"},{"language":"ja","languageUrl":"https://ja.aliexpress.com/item/1739481085.html"},{"language":"ko","languageUrl":"https://ko.aliexpress.com/item/1739481085.html"},{"language":"nl","languageUrl":"https://nl.aliexpress.com/item/1739481085.html"},{"language":"ar","languageUrl":"https://ar.aliexpress.com/item/1739481085.html"},{"language":"tr","languageUrl":"https://tr.aliexpress.com/item/1739481085.html"},{"language":"vi","languageUrl":"https://vi.aliexpress.com/item/1739481085.html"},{"language":"he","languageUrl":"https://he.aliexpress.com/item/1739481085.html"},{"language":"th","languageUrl":"https://th.aliexpress.com/item/1739481085.html"},{"language":"pl","languageUrl":"https://pl.aliexpress.com/item/1739481085.html"}],"name":"PageModule","ogDescription":"Smarter Shopping, Better Living!  Aliexpress.com","ogTitle":"8.7US $ 11% OFF|10 set  2/3/4/5/6/7/8/9/10 pin aviation  connector XLR Audio Cable Connector male and female  Panel Chassis Mount Kit  16mm|connector male|connectors xlrconnector set - AliExpress","ogurl":"//www.aliexpress.com/item/1739481085.html","oldItemDetailUrl":"https://www.aliexpress.com/item/10-Pcs-6pin-XLR-Audio-Cable-Connector-16mm-Chassis-Mount/1739481085.html","plazaElectronicSeller":false,"productId":1739481085,"ruSelfOperation":false,"showPlazaHeader":false,"siteType":"glo","spanishPlaza":false,"title":"10 set  2/3/4/5/6/7/8/9/10 pin aviation  connector XLR Audio Cable Connector male and female  Panel Chassis Mount Kit  16mm|connector male|connectors xlrconnector set - AliExpress"},"preSaleModule":{"features":{},"i18nMap":{},"id":0,"name":"PreSaleModule","preSale":false},"prefix":"//assets.alicdn.com/g/ae-fe/detail-ui/0.0.52","priceModule":{"activity":true,"bigPreview":false,"bigSellProduct":false,"discount":11,"discountPromotion":true,"features":{},"formatedActivityPrice":"US $8.70 - 18.89","formatedPrice":"US $9.77 - 21.22","hiddenBigSalePrice":false,"i18nMap":{"LOT":"lot","INSTALLMENT":"Installment","DEPOSIT":"Deposit","PRE_ORDER_PRICE":"Pre-order price"},"id":0,"installment":false,"lot":true,"maxActivityAmount":{"currency":"USD","formatedAmount":"US $18.89","value":18.89},"maxAmount":{"currency":"USD","formatedAmount":"US $21.22","value":21.22},"minActivityAmount":{"currency":"USD","formatedAmount":"US $8.70","value":8.7},"minAmount":{"currency":"USD","formatedAmount":"US $9.77","value":9.77},"multiUnitName":"Sets","name":"PriceModule","numberPerLot":10,"oddUnitName":"Set","preSale":false,"regularPriceActivity":false,"showActivityMessage":false},"quantityModule":{"activity":true,"displayBulkInfo":false,"features":{},"i18nMap":{"LOT":"lot","LOTS":"lots","BUY_LIMIT":"{limit} {unit} at most per customer","QUANTITY":"Quantity","OFF_OR_MORE":"{discount}% off ({number} {unit} or more)","ONLY_QUANTITY_LEFT":"Only {availQuantity} left","ADDTIONAL":"Additional","QUANTITY_AVAILABLE":"{availQuantity} {unit} available"},"id":0,"lot":true,"multiUnitName":"Sets","name":"QuantityModule","oddUnitName":"Set","purchaseLimitNumMax":0,"totalAvailQuantity":8548},"recommendModule":{"categoryId":200001565,"companyId":110097067,"features":{"recommendGpsScenarioOtherSellerProducts":"pcDetailBottomMoreOtherSeller","showSubTitle":"true","recommendGpsScenarioTopSelling":"pcDetailLeftTopSell","recommendGpsScenarioSellerOtherProducts":"pcDetailBottomMoreThisSeller"},"i18nMap":{"MORE_FROM_THIS_SELLER":"Seller Recommendations","YOU_MAY_LIKE":"Recommended For You","TOP_SELLING":"Top Selling","FROM_OTHER_SELLER":"More To Love","VIEW_MORE_LINK":"View More","PRODUCT_SOLD":"Sold"},"id":0,"name":"RecommendModule","platformCount":20,"productId":1739481085,"storeNum":408908},"redirectModule":{"bigBossBan":false,"code":"OK","features":{},"i18nMap":{},"id":0,"name":"RedirectModule","redirectUrl":""},"shippingModule":{"currencyCode":"USD","features":{},"hbaFreeShipping":false,"hbaFreight":false,"i18nMap":{"FAST_SHIPPING":"Fast Shipping","CAN_NOT_DELIVER":"Can not deliver","HBA_TRAKING_AVAILABLE":"Tracking Available","HAB_BALLOON_TRAKING_AVAILABLE":"Track your order status from start to finish.","SELECT_SHIP_FROM_TIP":"Please select the country you want to ship from","DAYS":"days","SHIPPING_TO":"Shipping:","HAB_SHIPPING_TO":"to","CARRIER":"Carrier","FREE_SHIPPING":"Free Shipping","COST":"Cost","BALLOON_TIP":"If you finish the payment today, your order will arrive within the estimated delivery time.","SHIP_MY_ITEM_TO":"Ship to","HAB_BALLOON_VAT_INCLUDED":"Item price listed includes VAT.","TO_PROVINCE":"To {provinceName}","TRACKING":"Tracking","LOGISTIC_COMPOSE_SPEED_UP":"Speed up to ","TO_COUNTRY":"to {countryName}","TO_CITY":"To {cityName}","CAN_NOE_DELIVER_NOTE":"This Supplier/Shipping Company does not deliver to your selected Country/Region.","ESTIMATED_DELIVERT_ON_DAYS":"Estimated Delivery: {0} days","CHOOSE_DELIVERT_METHOD":"Shipping Method","HAB_BALLOON_DOOR_DELIVERY":"Products delivered directly to your door.","DELIVERED_BY":"Delivered before {date} or full refund","HBA_SHIPPING_INFO":"To {countryName} in {time} days via {companyName}","PLAZA_BALLOON_TIP":"This delivery date is estimated. The calculation is based on several factors, including the address, shipping option selected and the availability shown on the product detail page.","IN":"in","SEARCH":"State/Province/Region","SELECT_SHIP_FROM":"Please select the country you want to ship from","HBA_TVAT_INCLUDED":"With sterile service","LOGISTIC_COMPOSE_AE":"Powered by AliExpress","ESTIMATED_DELIVERY":"Estimated Delivery","HBA_BALLOON_TIPS":"hba balloon tips","VAT_DE_DETAIL":"Buyer is German import declarant","SELECTED":"Selected","TO_WHERE":"To where","HBA_DOR_DELIVERY":"COD Available on APP","VAT_NUMBER":"VAT number:","ESTIMATED_DELIVERT_ON_DATE":"Estimated Delivery on {date}","OR_FULL_REFOUND":"Full refund","TO_VIA":"to {countryName} via {companyName}","APPLY":"Apply","LOGISTIC_COMPOSE_BRAND_MIND":"Combined Delivery","PLAZA_SHOP_NOW_RECEIVE_ON":"Buy it now and receive it on {date} (est.)","LOGISTIC_COMPOSE_ORDERS_OVER":"On orders over {0}"},"id":0,"name":"ShippingModule","productId":1739481085,"regionCityName":"Babenki","regionCountryName":"Russian Federation","regionProvinceName":"Moscow","userCountryCode":"CN","userCountryName":"China (Mainland)"},"skuModule":{"categoryId":200001565,"features":{},"forcePromiseWarrantyJson":"{}","hasSizeInfo":false,"hasSkuProperty":true,"i18nMap":{"SIZING_INFO":"Size Info","BUST_PROMPT":"Please select your bust size.","GLASSES_DIALOG_TITLE":"Prescription Detail","NV_ADD":"Sometimes seen on your prescription as \"NV, ADD, Near, Reading, or Reading Addition.\"  \"NV\" stands for \"Near-Vision.\"  This number indicates the additional magnification that is added to the distance prescription to get the reading portion of the lens in a multi-focal prescription.  We display a single NV-ADD field since it is almost always the same value for both eyes.","SPH":"SPHERE (SPH), or Spherical, refers to the refractive correction in the prescription. Minus (-) stands for near sightedness, and Plus (+) stands for far sightedness. If \"PL\" or \"Plano\" is written for the either SPH value on your prescription, you should select a value of 0.00.","PUPILLARY_PROMPT":"Please select PD (Pupillary Distance).","SIZE_HOVER_TITLE":"Might be different from your local size,please see the sizing info for more information.","FLOOR_CONTENT":"Begin at the hollow space between the collarbones and pull tape straight down to the floor.","CUSTOM_SIZE_CONTENT":"Your body measurements","NV_ADD_PROMPT":"Please select NV-ADD.","PLEASE_ENTER":"please enter","WAIST_TITLE":"Waist","WAIST_CONTENT":"Measure the smallest part of the waist.","CYL":"CYLINDER (CYL), or Cylinder, refers to the strength of the correction for the astigmatism in the eye. It can be either positive or negative. If there is a CYL value for an eye, there must be an Axis value for that eye.","SERVICE":"Service","BUST_CONTENT":"Wear an unpadded bra (your dress will have a built-in bra).","SIZE_INFO":"Size Info","SIZE_INFO_DESC":"*These charts are for reference only. Fit may vary depending on the construction, materials and manufacturer.","ITEM_CONDITION_TIP":"Condition","BTN_CANCEL":"Cancel","HOW_TO_MEASURE":"How to Measure","SIZE_INFO_TIP":"once you know your body measurements,consult the Size Chart on the product pages for actual item measurements to determine which size you should purchase.","FLOOR_PROMPT":"Please select the hollow to floor measurement.","FLOOR_TITLE":"Hollow to Floor (Bare Foot)","SIZE_INFO_COMPARE_TIP":"To choose the correct size for you measure your body as follows","UNIT_PROMPT":"Please select unit.","SELECT":"Select","HIPS_TITLE":"Hips","HEIGHT_PROMPT":"Please select the your shoes' heel height.","WAIST_PROMPT":"Please select your waist size.","BTN_SAVE":"Save","TITLE_OPTIONAL":"Local repair warranty in {country} <span>(optional)</span>","SIZE_DIALOG_TITLE":"Custom Size","GLASSES_TIP":"Please provide the information from your medical prescription. If you have any special needs or have any questions, please contact the seller.","SIZE_CHART":"Size Chart","HIPS_PROMPT":"Please select your hips size.","SPH_PROMPT":"It looks like you forgot to enter your prescription. Please fill in the sphere, cylinder and axis.","HIPS_CONTENT":"Find the widest part of the hips or run the measurement tape around your hipbone.","BUST_TITLE":"Bust","AXIS":"AXS, or Axis, refers to the angle of the correction for the astigmatism in the eye (if one exists) from 1 to 180.  If there is an Axis value on an eye, there must be a CYL (Cylinder) value on that eye.  If there is no Cylinder value or if the value is zero, the OD Axis value is entered as 0.00."},"id":0,"name":"SKUModule","productSKUPropertyList":[{"isShowTypeColor":false,"order":2,"showType":"none","showTypeColor":false,"skuPropertyId":14,"skuPropertyName":"Color","skuPropertyValues":[{"propertyValueDefinitionName":"2 Pin","propertyValueDisplayName":"2 Pin","propertyValueId":200004889,"propertyValueIdLong":200004889,"propertyValueName":"Army Green","skuColorValue":"#7C8C30","skuPropertyTips":"2 Pin","skuPropertyValueShowOrder":2,"skuPropertyValueTips":"2 Pin"},{"propertyValueDefinitionName":"3 Pin","propertyValueDisplayName":"3 Pin","propertyValueId":200004890,"propertyValueIdLong":200004890,"propertyValueName":"Dark Grey","skuColorValue":"#666","skuPropertyTips":"3 Pin","skuPropertyValueShowOrder":2,"skuPropertyValueTips":"3 Pin"},{"propertyValueDefinitionName":"4 Pin","propertyValueDisplayName":"4 Pin","propertyValueId":200004891,"propertyValueIdLong":200004891,"propertyValueName":"Lavender","skuColorValue":"#DDA0DD","skuPropertyTips":"4 Pin","skuPropertyValueShowOrder":2,"skuPropertyValueTips":"4 Pin"},{"propertyValueDefinitionName":"5 Pin","propertyValueDisplayName":"5 Pin","propertyValueId":200003699,"propertyValueIdLong":200003699,"propertyValueName":"MULTI","skuColorValue":"mutil","skuPropertyTips":"5 Pin","skuPropertyValueShowOrder":2,"skuPropertyValueTips":"5 Pin"},{"propertyValueDefinitionName":"6 Pin","propertyValueDisplayName":"6 Pin","propertyValueId":200002130,"propertyValueIdLong":200002130,"propertyValueName":"Ivory","skuColorValue":"#FDFDE8","skuPropertyTips":"6 Pin","skuPropertyValueShowOrder":2,"skuPropertyValueTips":"6 Pin"},{"propertyValueDefinitionName":"7 Pin","propertyValueDisplayName":"7 Pin","propertyValueId":365458,"propertyValueIdLong":365458,"propertyValueName":"Brown","skuColorValue":"#8D6468","skuPropertyTips":"7 Pin","skuPropertyValueShowOrder":2,"skuPropertyValueTips":"7 Pin"},{"propertyValueDefinitionName":"8 Pin","propertyValueDisplayName":"8 Pin","propertyValueId":350850,"propertyValueIdLong":350850,"propertyValueName":"Gold","skuColorValue":"#FFD700","skuPropertyTips":"8 Pin","skuPropertyValueShowOrder":2,"skuPropertyValueTips":"8 Pin"},{"propertyValueDefinitionName":"9 Pin","propertyValueDisplayName":"9 Pin","propertyValueId":350852,"propertyValueIdLong":350852,"propertyValueName":"Orange","skuColorValue":"#FFA500","skuPropertyTips":"9 Pin","skuPropertyValueShowOrder":2,"skuPropertyValueTips":"9 Pin"},{"propertyValueDefinitionName":"10 Pin","propertyValueDisplayName":"10 Pin","propertyValueId":350853,"propertyValueIdLong":350853,"propertyValueName":"Silver","skuColorValue":"#CCC","skuPropertyTips":"10 Pin","skuPropertyValueShowOrder":2,"skuPropertyValueTips":"10 Pin"}]}],"skuPriceList":[{"freightExt":"{\"p0\":\"65193513629\",\"p1\":\"18.89\",\"p3\":\"USD\",\"p4\":\"990000\",\"p5\":\"0\"}","skuAttr":"14:350853#10 Pin","skuId":65193513629,"skuIdStr":"65193513629","skuPropIds":"350853","skuVal":{"actSkuCalPrice":"18.89","actSkuMultiCurrencyCalPrice":"18.89","actSkuMultiCurrencyDisplayPrice":"18.89","availQuantity":987,"inventory":987,"isActivity":true,"optionalWarrantyPrice":[],"skuActivityAmount":{"currency":"USD","formatedAmount":"US $18.89","value":18.89},"skuAmount":{"currency":"USD","formatedAmount":"US $21.22","value":21.22},"skuCalPrice":"21.22","skuMultiCurrencyCalPrice":"21.22","skuMultiCurrencyDisplayPrice":"21.22","skuMultiCurrencyPerPiecePrice":"1.89"}},{"freightExt":"{\"p0\":\"64258825383\",\"p1\":\"8.77\",\"p3\":\"USD\",\"p4\":\"990000\",\"p5\":\"0\"}","skuAttr":"14:200004889#2 Pin","skuId":64258825383,"skuIdStr":"64258825383","skuPropIds":"200004889","skuVal":{"actSkuCalPrice":"8.77","actSkuMultiCurrencyCalPrice":"8.77","actSkuMultiCurrencyDisplayPrice":"8.77","availQuantity":970,"inventory":970,"isActivity":true,"optionalWarrantyPrice":[],"skuActivityAmount":{"currency":"USD","formatedAmount":"US $8.77","value":8.77},"skuAmount":{"currency":"USD","formatedAmount":"US $9.85","value":9.85},"skuCalPrice":"9.85","skuMultiCurrencyCalPrice":"9.85","skuMultiCurrencyDisplayPrice":"9.85","skuMultiCurrencyPerPiecePrice":"0.88"}},{"freightExt":"{\"p0\":\"64258825385\",\"p1\":\"9.81\",\"p3\":\"USD\",\"p4\":\"990000\",\"p5\":\"0\"}","skuAttr":"14:200004891#4 Pin","skuId":64258825385,"skuIdStr":"64258825385","skuPropIds":"200004891","skuVal":{"actSkuCalPrice":"9.81","actSkuMultiCurrencyCalPrice":"9.81","actSkuMultiCurrencyDisplayPrice":"9.81","availQuantity":905,"inventory":905,"isActivity":true,"optionalWarrantyPrice":[],"skuActivityAmount":{"currency":"USD","formatedAmount":"US $9.81","value":9.81},"skuAmount":{"currency":"USD","formatedAmount":"US $11.02","value":11.02},"skuCalPrice":"11.02","skuMultiCurrencyCalPrice":"11.02","skuMultiCurrencyDisplayPrice":"11.02","skuMultiCurrencyPerPiecePrice":"0.99"}},{"freightExt":"{\"p0\":\"64258825384\",\"p1\":\"8.70\",\"p3\":\"USD\",\"p4\":\"990000\",\"p5\":\"0\"}","skuAttr":"14:200004890#3 Pin","skuId":64258825384,"skuIdStr":"64258825384","skuPropIds":"200004890","skuVal":{"actSkuCalPrice":"8.70","actSkuMultiCurrencyCalPrice":"8.7","actSkuMultiCurrencyDisplayPrice":"8.70","availQuantity":911,"inventory":911,"isActivity":true,"optionalWarrantyPrice":[],"skuActivityAmount":{"currency":"USD","formatedAmount":"US $8.70","value":8.7},"skuAmount":{"currency":"USD","formatedAmount":"US $9.77","value":9.77},"skuCalPrice":"9.77","skuMultiCurrencyCalPrice":"9.77","skuMultiCurrencyDisplayPrice":"9.77","skuMultiCurrencyPerPiecePrice":"0.87"}},{"freightExt":"{\"p0\":\"64258825387\",\"p1\":\"10.72\",\"p3\":\"USD\",\"p4\":\"990000\",\"p5\":\"0\"}","skuAttr":"14:200002130#6 Pin","skuId":64258825387,"skuIdStr":"64258825387","skuPropIds":"200002130","skuVal":{"actSkuCalPrice":"10.72","actSkuMultiCurrencyCalPrice":"10.72","actSkuMultiCurrencyDisplayPrice":"10.72","availQuantity":947,"inventory":947,"isActivity":true,"optionalWarrantyPrice":[],"skuActivityAmount":{"currency":"USD","formatedAmount":"US $10.72","value":10.72},"skuAmount":{"currency":"USD","formatedAmount":"US $12.04","value":12.04},"skuCalPrice":"12.04","skuMultiCurrencyCalPrice":"12.04","skuMultiCurrencyDisplayPrice":"12.04","skuMultiCurrencyPerPiecePrice":"1.08"}},{"freightExt":"{\"p0\":\"64258825386\",\"p1\":\"10.72\",\"p3\":\"USD\",\"p4\":\"990000\",\"p5\":\"0\"}","skuAttr":"14:200003699#5 Pin","skuId":64258825386,"skuIdStr":"64258825386","skuPropIds":"200003699","skuVal":{"actSkuCalPrice":"10.72","actSkuMultiCurrencyCalPrice":"10.72","actSkuMultiCurrencyDisplayPrice":"10.72","availQuantity":943,"inventory":943,"isActivity":true,"optionalWarrantyPrice":[],"skuActivityAmount":{"currency":"USD","formatedAmount":"US $10.72","value":10.72},"skuAmount":{"currency":"USD","formatedAmount":"US $12.04","value":12.04},"skuCalPrice":"12.04","skuMultiCurrencyCalPrice":"12.04","skuMultiCurrencyDisplayPrice":"12.04","skuMultiCurrencyPerPiecePrice":"1.08"}},{"freightExt":"{\"p0\":\"64258825389\",\"p1\":\"11.62\",\"p3\":\"USD\",\"p4\":\"990000\",\"p5\":\"0\"}","skuAttr":"14:350850#8 Pin","skuId":64258825389,"skuIdStr":"64258825389","skuPropIds":"350850","skuVal":{"actSkuCalPrice":"11.62","actSkuMultiCurrencyCalPrice":"11.62","actSkuMultiCurrencyDisplayPrice":"11.62","availQuantity":934,"inventory":934,"isActivity":true,"optionalWarrantyPrice":[],"skuActivityAmount":{"currency":"USD","formatedAmount":"US $11.62","value":11.62},"skuAmount":{"currency":"USD","formatedAmount":"US $13.06","value":13.06},"skuCalPrice":"13.06","skuMultiCurrencyCalPrice":"13.06","skuMultiCurrencyDisplayPrice":"13.06","skuMultiCurrencyPerPiecePrice":"1.17"}},{"freightExt":"{\"p0\":\"64258825388\",\"p1\":\"11.62\",\"p3\":\"USD\",\"p4\":\"990000\",\"p5\":\"0\"}","skuAttr":"14:365458#7 Pin","skuId":64258825388,"skuIdStr":"64258825388","skuPropIds":"365458","skuVal":{"actSkuCalPrice":"11.62","actSkuMultiCurrencyCalPrice":"11.62","actSkuMultiCurrencyDisplayPrice":"11.62","availQuantity":975,"inventory":975,"isActivity":true,"optionalWarrantyPrice":[],"skuActivityAmount":{"currency":"USD","formatedAmount":"US $11.62","value":11.62},"skuAmount":{"currency":"USD","formatedAmount":"US $13.06","value":13.06},"skuCalPrice":"13.06","skuMultiCurrencyCalPrice":"13.06","skuMultiCurrencyDisplayPrice":"13.06","skuMultiCurrencyPerPiecePrice":"1.17"}},{"freightExt":"{\"p0\":\"64258825390\",\"p1\":\"12.53\",\"p3\":\"USD\",\"p4\":\"990000\",\"p5\":\"0\"}","skuAttr":"14:350852#9 Pin","skuId":64258825390,"skuIdStr":"64258825390","skuPropIds":"350852","skuVal":{"actSkuCalPrice":"12.53","actSkuMultiCurrencyCalPrice":"12.53","actSkuMultiCurrencyDisplayPrice":"12.53","availQuantity":976,"inventory":976,"isActivity":true,"optionalWarrantyPrice":[],"skuActivityAmount":{"currency":"USD","formatedAmount":"US $12.53","value":12.53},"skuAmount":{"currency":"USD","formatedAmount":"US $14.08","value":14.08},"skuCalPrice":"14.08","skuMultiCurrencyCalPrice":"14.08","skuMultiCurrencyDisplayPrice":"14.08","skuMultiCurrencyPerPiecePrice":"1.26"}}],"warrantyDetailJson":"[]"},"specsModule":{"features":{},"i18nMap":{},"id":0,"name":"SpecsModule","props":[{"attrName":"Model Number","attrNameId":3,"attrValue":"RN-GX16","attrValueId":"-1"},{"attrName":"Brand Name","attrNameId":2,"attrValue":"MAXRUINOR","attrValueId":"202307850"}]},"storeModule":{"buyerAdminSeq":240193246,"companyId":110097067,"countryCompleteName":"China","detailPageUrl":"//www.aliexpress.com/item/1739481085.html","esRetailOrConsignment":false,"features":{},"feedbackMessageServer":"//message.aliexpress.com","feedbackServer":"//feedback.aliexpress.com","followed":false,"followingNumber":133,"hasStore":true,"hasStoreHeader":true,"hideCustomerService":false,"i18nMap":{"COUSTOMER_SERVICE":"Customer Service","VISIT_STORE":"Visit Store","CONTACT_SELLER":"Contact","FOLLOWING_STATE":"Following","UNFOLLOWING_STATE":"Follow","POSITIVE_FEEDBACK":"Positive Feedback","FOLLOWERS":"Followers","FOLLOWER":"Follower","TOP_SELLER":"Top Brands","STORE_CATEGORIES":"Store Categories"},"id":0,"name":"StoreModule","openTime":"Oct 8, 2011","openedYear":9,"positiveNum":166,"positiveRate":"100.0%","productId":1739481085,"province":"Guangdong","sellerAdminSeq":118739230,"sessionId":"1ptIGYPgqTQCAXF0Ip7M3TZT","siteType":"glo","storeName":"Shenzhen aliElectric Technology Co., Ltd","storeNum":408908,"storeURL":"//www.aliexpress.com/store/408908","topBrandDescURL":"https://sale.aliexpress.com/topbrand.htm","topRatedSeller":false},"titleModule":{"features":{},"feedbackRating":{"averageStar":"5.0","averageStarRage":"100.0","display":true,"evarageStar":"5.0","evarageStarRage":"100.0","fiveStarNum":1,"fiveStarRate":"100","fourStarNum":0,"fourStarRate":"0","oneStarNum":0,"oneStarRate":"0","positiveRate":"100.0","threeStarNum":0,"threeStarRate":"0","totalValidNum":1,"trialReviewNum":0,"twoStarNum":0,"twoStarRate":"0"},"formatTradeCount":"4","i18nMap":{"REVIEWS":"Reviews","VIEW_ALL_REVIEWS":"View All Reviews","REVIEW":"Review","VIEW_OTHER_TITLE":"View title in multi-language (machine translated)","VIEW_EN_TITLE":"View original title in English","FREEBIE_REVIEW":"Review Of Freebies ","FREEBIE_REVIEWS":"Reviews Of Freebies "},"id":0,"name":"TitleModule","orig":false,"origTitle":false,"subject":"10 set  2/3/4/5/6/7/8/9/10 pin aviation  connector XLR Audio Cable Connector male and female  Panel Chassis Mount Kit  16mm","tradeCount":4,"tradeCountUnit":"orders","trans":false,"transTitle":false},"webEnv":{"country":"RU","currency":"USD","env":{"valMap":{"g11n:locale":"zh_CN","g11n:timezone":"","ua:device":"pc","user:id":"240193246","g11n:country":"RU","page:name":"","g11n:site":"glo","page:app":"","ua:browser":"chrome","ua:platform":"other","user:type":"","page:id":"item_html","user:member":"","g11n:currency":"USD"},"zone":"global_env"},"host":"www.aliexpress.com","hostname":"ae-glodetail-web011010233096.center.na61","ip":"119.123.100.213","lang":"en_US","language":"en","locale":"en_US","reqHost":"https://www.aliexpress.com","site":"glo"}}
    /// </summary>
    public class SkuModule: SiteBaseProduct
    {
        /// <summary>
        /// 
        /// </summary>
        public string categoryId { get; set; }
        /// <summary>
        /// 
        /// </summary>


        /// <summary>
        /// 
        /// </summary>
        public bool hasSizeInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool hasSkuProperty { get; set; }
        /// <summary>
        /// 
        /// </summary>

        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ProductSKUPropertyList> productSKUPropertyList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SkuPriceList> skuPriceList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string warrantyDetailJson { get; set; }
    }

    public class Props
    {
        /// <summary>
        /// 
        /// </summary>
        public string attrName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string attrNameId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string attrValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string attrValueId { get; set; }
    }

    public class SpecsModule
    {

        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Props> props { get; set; }
    }










}
