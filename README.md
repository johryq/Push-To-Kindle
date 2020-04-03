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

5.编码

```bash
信息(prcgen):I1045: 本书中使用 UNICODE 范围计算
信息(prcgen):I1046: 已发现的 UNICODE 范围：Basic Latin [20..7E]
信息(prcgen):I1046: 已发现的 UNICODE 范围：CJK Unified Ideographs [4E00..9FFF]
信息(prcgen):I1046: 已发现的 UNICODE 范围：General Punctuation - Windows 1252 [201C..201E]
信息(prcgen):I1046: 已发现的 UNICODE 范围：Halfwidth and Fullwidth Forms [FF00..FFEF]
信息(prcgen):I1046: 已发现的 UNICODE 范围：Chinese, Japanese, and Korean (CJK) Symbols and Punctuation [3000..303F]
信息(prcgen):I1046: 已发现的 UNICODE 范围：Letter-like Symbols [2100..214F]
信息(prcgen):I1046: 已发现的 UNICODE 范围：Latin Extended-A [100..17F]
```
