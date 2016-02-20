# PingMaster
方便Ping大量地址做的一个小工具，尤其方便PingVPN

# 使用方法
配置list.txt 其实就是csv格式文件，GoogleSheet Excel都方便编辑

# 格式
[名称],[地址]
例如：
网易,www.163.com

# Tips
暂时方便用 VS运行所以配置文件写成了StreamReader sr = new StreamReader("../../list.txt", Encoding.Default);
想要配置文件和exe文件同目录请改成StreamReader sr = new StreamReader("./list.txt", Encoding.Default);
