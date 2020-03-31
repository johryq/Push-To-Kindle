# 向我的Kindle邮箱推送小说

## KindleGen的坑

1.`错误(htmlparser):E19001: 在内容文件中没有发现 BODY 标记。`

```xml
<item id="text" media-type="text/x-oeb1-document" href="index.html"></item>
``` 

或者

```xml
<?xml version="1.0" encoding="UTF-8"?>
```

> media-type属性错误 应该为 `text/css`

2.HTMl中文乱码  

Charset应该设置成

``` html

<meta http-equiv="content-type" content="text/html; charset=UTF-8">
```
