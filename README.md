# 向我的Kindle邮箱推送小说

## KindleGen的坑

1.`错误(prcgen):E24010:无法解析目录中的超链接（一个可能的原因是此链接指向带“样式显示：无 (style display:none)”的标签）index.html#dir2`
> a 标签 src属性 结束 " 多了个空格

---
2.`警告(inputpreprocessor):W29004: 强制关闭的已打开标签为：`
> 结束标签空一格,具体参考 [AmazonKindlePublishingGuidelines_CN](AmazonKindlePublishingGuidelines_CN.pdf)
---

3.`错误(htmlparser):E19001: 在内容文件中没有发现 BODY 标记。`

``` xml
<item id="text" media-type="text/x-oeb1-document" href="index.html"></item>
```

> media-type属性错误 应该为 `text/css`

---
4.HTMl中文乱码  

Charset应该设置成

```xml
<meta http-equiv="content-type" content="text/html; charset=UTF-8">
```

或者

```xml
<?xml version="1.0" encoding="UTF-8"?>
```
