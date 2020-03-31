# 向我的Kindle邮箱推送小说

## KindleGen的坑

1.`错误(htmlparser):E19001: 在内容文件中没有发现 BODY 标记。`

``` html
<meta http-equiv="content-type" content="text/html; charset=UTF-8">
```

> media-type属性错误 应该为 `text/css`

---
2.HTMl中文乱码  

Charset应该设置成

```xml
<item id="text" media-type="text/x-oeb1-document" href="index.html"></item>
```

或者

```xml
<?xml version="1.0" encoding="UTF-8"?>
```



---
3.`警告(inputpreprocessor):W29004: 强制关闭的已打开标签为：`
> 结束标签空一格,具体参考 [AmazonKindlePublishingGuidelines_CN](AmazonKindlePublishingGuidelines_CN.pdf)
