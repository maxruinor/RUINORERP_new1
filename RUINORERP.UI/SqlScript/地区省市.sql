-- 插入地区数据
INSERT INTO dbo.tb_CRM_Region (Region_ID, Region_Name, Sort)
VALUES
(1, '华北地区', 1),
(2, '东北地区', 2),
(3, '华东地区', 3),
(4, '华中地区', 4),
(5, '华南地区', 5),
(6, '西南地区', 6),
(7, '西北地区', 7),
(8, '其他地区', 8);

GO

-- 插入地区对应的省份数据
INSERT INTO dbo.tb_Provinces (ProvinceID, Region_ID, ProvinceCNName, ProvinceENName)
VALUES
(1, 1, '北京', 'Beijing'),
(2, 1, '天津', 'Tianjin'),
(3, 1, '河北', 'Hebei'),
(4, 1, '山西', 'Shanxi'),
(5, 1, '内蒙古', 'Inner Mongolia'),
(6, 2, '辽宁', 'Liaoning'),
(7, 2, '吉林', 'Jilin'),
(8, 2, '黑龙江', 'Heilongjiang'),
(9, 3, '上海', 'Shanghai'),
(10, 3, '江苏', 'Jiangsu'),
(11, 3, '浙江', 'Zhejiang'),
(12, 3, '安徽', 'Anhui'),
(13, 3, '福建', 'Fujian'),
(14, 3, '江西', 'Jiangxi'),
(15, 3, '山东', 'Shandong'),
(16, 4, '河南', 'Henan'),
(17, 4, '湖北', 'Hubei'),
(18, 4, '湖南', 'Hunan'),
(19, 5, '广东', 'Guangdong'),
(20, 5, '广西', 'Guangxi'),
(21, 5, '海南', 'Hainan'),
(22, 6, '重庆', 'Chongqing'),
(23, 6, '四川', 'Sichuan'),
(24, 6, '贵州', 'Guizhou'),
(25, 6, '云南', 'Yunnan'),
(26, 6, '西藏', 'Tibet'),
(27, 7, '陕西', 'Shaanxi'),
(28, 7, '甘肃', 'Gansu'),
(29, 7, '青海', 'Qinghai'),
(30, 7, '宁夏', 'Ningxia'),
(31, 7, '新疆', 'Xinjiang'),
(32, 8, '台湾', 'Taiwan'),
(33, 8, '香港', 'Hong Kong'),
(34, 8, '澳门', 'Macau'),
(35, 8, '国外', 'Abroad');

GO

-- Beijing (1)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(101, 1, '东城区', 'Dongcheng District'),
(102, 1, '西城区', 'Xicheng District'),
(103, 1, '朝阳区', 'Chaoyang District'),
(104, 1, '丰台区', 'Fengtai District'),
(105, 1, '石景山区', 'Shijingshan District'),
(106, 1, '海淀区', 'Haidian District'),
(107, 1, '顺义区', 'Shunyi District'),
(108, 1, '通州区', 'Tongzhou District'),
(109, 1, '大兴区', 'Daxing District'),
(110, 1, '房山区', 'Fangshan District'),
(111, 1, '门头沟区', 'Mentougou District'),
(112, 1, '昌平区', 'Changping District'),
(113, 1, '平谷区', 'Pinggu District'),
(114, 1, '密云区', 'Miyun District'),
(115, 1, '怀柔区', 'Huairou District'),
(116, 1, '延庆区', 'Yanqing District');

-- Tianjin (2)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(201, 2, '和平区', 'Heping District'),
(202, 2, '河东区', 'Hedong District'),
(203, 2, '河西区', 'Hexi District'),
(204, 2, '南开区', 'Nankai District'),
(205, 2, '河北区', 'Hebei District'),
(206, 2, '红桥区', 'Hongqiao District'),
(207, 2, '东丽区', 'Dongli District'),
(208, 2, '西青区', 'Xiqing District'),
(209, 2, '津南区', 'Jinnan District'),
(210, 2, '北辰区', 'Beichen District'),
(211, 2, '武清区', 'Wuqing District'),
(212, 2, '宝坻区', 'Baodi District'),
(213, 2, '滨海新区', 'Binhai New Area'),
(214, 2, '宁河区', 'Ninghe District'),
(215, 2, '静海区', 'Jinghai District'),
(216, 2, '蓟州区', 'Jizhou District');

-- Hebei (3)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(301, 3, '石家庄市', 'Shijiazhuang'),
(302, 3, '唐山市', 'Tangshan'),
(303, 3, '秦皇岛市', 'Qinhuangdao'),
(304, 3, '邯郸市', 'Handan'),
(305, 3, '邢台市', 'Xingtai'),
(306, 3, '保定市', 'Baoding'),
(307, 3, '张家口市', 'Zhangjiakou'),
(308, 3, '承德市', 'Chengde'),
(309, 3, '沧州市', 'Cangzhou'),
(310, 3, '廊坊市', 'Langfang'),
(311, 3, '衡水市', 'Hengshui');

-- Shanxi (4)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(401, 4, '太原市', 'Taiyuan'),
(402, 4, '大同市', 'Datong'),
(403, 4, '阳泉市', 'Yangquan'),
(404, 4, '长治市', 'Changzhi'),
(405, 4, '晋城市', 'Jincheng'),
(406, 4, '朔州市', 'Shuozhou'),
(407, 4, '晋中市', 'Jinzhong'),
(408, 4, '运城市', 'Yuncheng'),
(409, 4, '忻州市', 'Xinzhou'),
(410, 4, '临汾市', 'Linfen'),
(411, 4, '吕梁市', 'Lüliang');

-- Inner Mongolia (5)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(501, 5, '呼和浩特市', 'Hohhot'),
(502, 5, '包头市', 'Baotou'),
(503, 5, '乌海市', 'Wuhai'),
(504, 5, '赤峰市', 'Chifeng'),
(505, 5, '通辽市', 'Tongliao'),
(506, 5, '鄂尔多斯市', 'Ordos'),
(507, 5, '呼伦贝尔市', 'Hulunbuir'),
(508, 5, '巴彦淖尔市', 'Bayan Nur'),
(509, 5, '乌兰察布市', 'Ulanqab'),
(510, 5, '兴安盟', 'Hinggan League'),
(511, 5, '锡林郭勒盟', 'Xilingol League'),
(512, 5, '阿拉善盟', 'Alxa League');

-- Liaoning (6)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(601, 6, '沈阳市', 'Shenyang'),
(602, 6, '大连市', 'Dalian'),
(603, 6, '鞍山市', 'Anshan'),
(604, 6, '抚顺市', 'Fushun'),
(605, 6, '本溪市', 'Benxi'),
(606, 6, '丹东市', 'Dandong'),
(607, 6, '锦州市', 'Jinzhou'),
(608, 6, '营口市', 'Yingkou'),
(609, 6, '阜新市', 'Fuxin'),
(610, 6, '辽阳市', 'Liaoyang'),
(611, 6, '盘锦市', 'Panjin'),
(612, 6, '铁岭市', 'Tieling'),
(613, 6, '朝阳市', 'Chaoyang'),
(614, 6, '葫芦岛市', 'Huludao');

-- Jilin (7)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(701, 7, '长春市', 'Changchun'),
(702, 7, '吉林市', 'Jilin City'),
(703, 7, '四平市', 'Siping'),
(704, 7, '辽源市', 'Liaoyuan'),
(705, 7, '通化市', 'Tonghua'),
(706, 7, '白山市', 'Baishan'),
(707, 7, '松原市', 'Songyuan'),
(708, 7, '白城市', 'Baicheng'),
(709, 7, '延边朝鲜族自治州', 'Yanbian Korean Autonomous Prefecture');

-- Heilongjiang (8)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(801, 8, '哈尔滨市', 'Harbin'),
(802, 8, '齐齐哈尔市', 'Qiqihar'),
(803, 8, '鸡西市', 'Jixi'),
(804, 8, '鹤岗市', 'Hegang'),
(805, 8, '双鸭山市', 'Shuangyashan'),
(806, 8, '大庆市', 'Daqing'),
(807, 8, '伊春市', 'Yichun'),
(808, 8, '佳木斯市', 'Jiamusi'),
(809, 8, '七台河市', 'Qitaihe'),
(810, 8, '牡丹江市', 'Mudanjiang'),
(811, 8, '黑河市', 'Heihe'),
(812, 8, '绥化市', 'Suihua'),
(813, 8, '大兴安岭地区', 'Daxinganling Prefecture');

-- Shanghai (9)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(901, 9, '黄浦区', 'Huangpu District'),
(902, 9, '徐汇区', 'Xuhui District'),
(903, 9, '长宁区', 'Changning District'),
(904, 9, '静安区', 'Jing''an District'),
(905, 9, '普陀区', 'Putuo District'),
(906, 9, '虹口区', 'Hongkou District'),
(907, 9, '杨浦区', 'Yangpu District'),
(908, 9, '闵行区', 'Minhang District'),
(909, 9, '宝山区', 'Baoshan District'),
(910, 9, '嘉定区', 'Jiading District'),
(911, 9, '金山区', 'Jinshan District'),
(912, 9, '松江区', 'Songjiang District'),
(913, 9, '青浦区', 'Qingpu District'),
(914, 9, '奉贤区', 'Fengxian District'),
(915, 9, '崇明区', 'Chongming District'),
(916, 9, '浦东新区', 'Pudong New Area');

-- Jiangsu (10)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(1001, 10, '南京市', 'Nanjing'),
(1002, 10, '无锡市', 'Wuxi'),
(1003, 10, '徐州市', 'Xuzhou'),
(1004, 10, '常州市', 'Changzhou'),
(1005, 10, '苏州市', 'Suzhou'),
(1006, 10, '南通市', 'Nantong'),
(1007, 10, '连云港市', 'Lianyungang'),
(1008, 10, '淮安市', 'Huai''an'),
(1009, 10, '盐城市', 'Yancheng'),
(1010, 10, '扬州市', 'Yangzhou'),
(1011, 10, '镇江市', 'Zhenjiang'),
(1012, 10, '泰州市', 'Taizhou'),
(1013, 10, '宿迁市', 'Suqian');

-- Zhejiang (11)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(1101, 11, '杭州市', 'Hangzhou'),
(1102, 11, '宁波市', 'Ningbo'),
(1103, 11, '温州市', 'Wenzhou'),
(1104, 11, '嘉兴市', 'Jiaxing'),
(1105, 11, '湖州市', 'Huzhou'),
(1106, 11, '绍兴市', 'Shaoxing'),
(1107, 11, '金华市', 'Jinhua'),
(1108, 11, '衢州市', 'Quzhou'),
(1109, 11, '舟山市', 'Zhoushan'),
(1110, 11, '台州市', 'Taizhou'),
(1111, 11, '丽水市', 'Lishui');

-- Anhui (12)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(1201, 12, '合肥市', 'Hefei'),
(1202, 12, '芜湖市', 'Wuhu'),
(1203, 12, '蚌埠市', 'Bengbu'),
(1204, 12, '淮南市', 'Huainan'),
(1205, 12, '马鞍山市', 'Ma''anshan'),
(1206, 12, '淮北市', 'Huaibei'),
(1207, 12, '铜陵市', 'Tongling'),
(1208, 12, '安庆市', 'Anqing'),
(1209, 12, '黄山市', 'Huangshan'),
(1210, 12, '滁州市', 'Chuzhou'),
(1211, 12, '阜阳市', 'Fuyang'),
(1212, 12, '宿州市', 'Suzhou'),
(1213, 12, '六安市', 'Lu''an'),
(1214, 12, '亳州市', 'Bozhou'),
(1215, 12, '池州市', 'Chizhou'),
(1216, 12, '宣城市', 'Xuancheng');

-- Fujian (13)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(1301, 13, '福州市', 'Fuzhou'),
(1302, 13, '厦门市', 'Xiamen'),
(1303, 13, '莆田市', 'Putian'),
(1304, 13, '三明市', 'Sanming'),
(1305, 13, '泉州市', 'Quanzhou'),
(1306, 13, '漳州市', 'Zhangzhou'),
(1307, 13, '南平市', 'Nanping'),
(1308, 13, '龙岩市', 'Longyan'),
(1309, 13, '宁德市', 'Ningde');

-- Jiangxi (14)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(1401, 14, '南昌市', 'Nanchang'),
(1402, 14, '景德镇市', 'Jingdezhen'),
(1403, 14, '萍乡市', 'Pingxiang'),
(1404, 14, '九江市', 'Jiujiang'),
(1405, 14, '新余市', 'Xinyu'),
(1406, 14, '鹰潭市', 'Yingtan'),
(1407, 14, '赣州市', 'Ganzhou'),
(1408, 14, '吉安市', 'Ji''an'),
(1409, 14, '宜春市', 'Yichun'),
(1410, 14, '抚州市', 'Fuzhou'),
(1411, 14, '上饶市', 'Shangrao');

-- Shandong (15)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(1501, 15, '济南市', 'Jinan'),
(1502, 15, '青岛市', 'Qingdao'),
(1503, 15, '淄博市', 'Zibo'),
(1504, 15, '枣庄市', 'Zaozhuang'),
(1505, 15, '东营市', 'Dongying'),
(1506, 15, '烟台市', 'Yantai'),
(1507, 15, '潍坊市', 'Weifang'),
(1508, 15, '济宁市', 'Jining'),
(1509, 15, '泰安市', 'Tai''an'),
(1510, 15, '威海市', 'Weihai'),
(1511, 15, '日照市', 'Rizhao'),
(1512, 15, '临沂市', 'Linyi'),
(1513, 15, '德州市', 'Dezhou'),
(1514, 15, '聊城市', 'Liaocheng'),
(1515, 15, '滨州市', 'Binzhou'),
(1516, 15, '菏泽市', 'Heze');

-- Henan (16)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(1601, 16, '郑州市', 'Zhengzhou'),
(1602, 16, '开封市', 'Kaifeng'),
(1603, 16, '洛阳市', 'Luoyang'),
(1604, 16, '平顶山市', 'Pingdingshan'),
(1605, 16, '安阳市', 'Anyang'),
(1606, 16, '鹤壁市', 'Hebi'),
(1607, 16, '新乡市', 'Xinxiang'),
(1608, 16, '焦作市', 'Jiaozuo'),
(1609, 16, '濮阳市', 'Puyang'),
(1610, 16, '许昌市', 'Xuchang'),
(1611, 16, '漯河市', 'Luohe'),
(1612, 16, '三门峡市', 'Sanmenxia'),
(1613, 16, '南阳市', 'Nanyang'),
(1614, 16, '商丘市', 'Shangqiu'),
(1615, 16, '信阳市', 'Xinyang'),
(1616, 16, '周口市', 'Zhoukou'),
(1617, 16, '驻马店市', 'Zhumadian'),
(1618, 16, '济源市', 'Jiyuan');

-- Hubei (17)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(1701, 17, '武汉市', 'Wuhan'),
(1702, 17, '黄石市', 'Huangshi'),
(1703, 17, '十堰市', 'Shiyan'),
(1704, 17, '宜昌市', 'Yichang'),
(1705, 17, '襄阳市', 'Xiangyang'),
(1706, 17, '鄂州市', 'Ezhou'),
(1707, 17, '荆门市', 'Jingmen'),
(1708, 17, '孝感市', 'Xiaogan'),
(1709, 17, '荆州市', 'Jingzhou'),
(1710, 17, '黄冈市', 'Huanggang'),
(1711, 17, '咸宁市', 'Xianning'),
(1712, 17, '随州市', 'Suizhou'),
(1713, 17, '恩施土家族苗族自治州', 'Enshi Tujia and Miao Autonomous Prefecture'),
(1714, 17, '仙桃市', 'Xiantao'),
(1715, 17, '潜江市', 'Qianjiang'),
(1716, 17, '天门市', 'Tianmen'),
(1717, 17, '神农架林区', 'Shennongjia Forestry District');

-- Hunan (18)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(1801, 18, '长沙市', 'Changsha'),
(1802, 18, '株洲市', 'Zhuzhou'),
(1803, 18, '湘潭市', 'Xiangtan'),
(1804, 18, '衡阳市', 'Hengyang'),
(1805, 18, '邵阳市', 'Shaoyang'),
(1806, 18, '岳阳市', 'Yueyang'),
(1807, 18, '常德市', 'Changde'),
(1808, 18, '张家界市', 'Zhangjiajie'),
(1809, 18, '益阳市', 'Yiyang'),
(1810, 18, '郴州市', 'Chenzhou'),
(1811, 18, '永州市', 'Yongzhou'),
(1812, 18, '怀化市', 'Huaihua'),
(1813, 18, '娄底市', 'Loudi'),
(1814, 18, '湘西土家族苗族自治州', 'Xiangxi Tujia and Miao Autonomous Prefecture');

-- Guangdong (19)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(1901, 19, '广州市', 'Guangzhou'),
(1902, 19, '韶关市', 'Shaoguan'),
(1903, 19, '深圳市', 'Shenzhen'),
(1904, 19, '珠海市', 'Zhuhai'),
(1905, 19, '汕头市', 'Shantou'),
(1906, 19, '佛山市', 'Foshan'),
(1907, 19, '江门市', 'Jiangmen'),
(1908, 19, '湛江市', 'Zhanjiang'),
(1909, 19, '茂名市', 'Maoming'),
(1910, 19, '肇庆市', 'Zhaoqing'),
(1911, 19, '惠州市', 'Huizhou'),
(1912, 19, '梅州市', 'Meizhou'),
(1913, 19, '汕尾市', 'Shanwei'),
(1914, 19, '河源市', 'Heyuan'),
(1915, 19, '阳江市', 'Yangjiang'),
(1916, 19, '清远市', 'Qingyuan'),
(1917, 19, '东莞市', 'Dongguan'),
(1918, 19, '中山市', 'Zhongshan'),
(1919, 19, '潮州市', 'Chaozhou'),
(1920, 19, '揭阳市', 'Jieyang'),
(1921, 19, '云浮市', 'Yunfu');

-- Guangxi (20)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(2001, 20, '南宁市', 'Nanning'),
(2002, 20, '柳州市', 'Liuzhou'),
(2003, 20, '桂林市', 'Guilin'),
(2004, 20, '梧州市', 'Wuzhou'),
(2005, 20, '北海市', 'Beihai'),
(2006, 20, '防城港市', 'Fangchenggang'),
(2007, 20, '钦州市', 'Qinzhou'),
(2008, 20, '贵港市', 'Guigang'),
(2009, 20, '玉林市', 'Yulin'),
(2010, 20, '百色市', 'Baise'),
(2011, 20, '贺州市', 'Hezhou'),
(2012, 20, '河池市', 'Hechi'),
(2013, 20, '来宾市', 'Laibin'),
(2014, 20, '崇左市', 'Chongzuo');

-- Hainan (21)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(2101, 21, '海口市', 'Haikou'),
(2102, 21, '三亚市', 'Sanya'),
(2103, 21, '三沙市', 'Sansha'),
(2104, 21, '儋州市', 'Danzhou'),
(2105, 21, '五指山市', 'Wuzhishan'),
(2106, 21, '琼海市', 'Qionghai'),
(2107, 21, '文昌市', 'Wenchang'),
(2108, 21, '万宁市', 'Wanning'),
(2109, 21, '东方市', 'Dongfang'),
(2110, 21, '定安县', 'Ding''an County'),
(2111, 21, '屯昌县', 'Tunchang County'),
(2112, 21, '澄迈县', 'Chengmai County'),
(2113, 21, '临高县', 'Lingao County'),
(2114, 21, '白沙黎族自治县', 'Baisha Li Autonomous County'),
(2115, 21, '昌江黎族自治县', 'Changjiang Li Autonomous County'),
(2116, 21, '乐东黎族自治县', 'Ledong Li Autonomous County'),
(2117, 21, '陵水黎族自治县', 'Lingshui Li Autonomous County'),
(2118, 21, '保亭黎族苗族自治县', 'Baoting Li and Miao Autonomous County'),
(2119, 21, '琼中黎族苗族自治县', 'Qiongzhong Li and Miao Autonomous County');

-- Chongqing (22)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(2201, 22, '万州区', 'Wanzhou District'),
(2202, 22, '涪陵区', 'Fuling District'),
(2203, 22, '渝中区', 'Yuzhong District'),
(2204, 22, '大渡口区', 'Dadukou District'),
(2205, 22, '江北区', 'Jiangbei District'),
(2206, 22, '沙坪坝区', 'Shapingba District'),
(2207, 22, '九龙坡区', 'Jiulongpo District'),
(2208, 22, '南岸区', 'Nan''an District'),
(2209, 22, '北碚区', 'Beibei District'),
(2210, 22, '綦江区', 'Qijiang District'),
(2211, 22, '大足区', 'Dazu District'),
(2212, 22, '渝北区', 'Yubei District'),
(2213, 22, '巴南区', 'Banan District'),
(2214, 22, '黔江区', 'Qianjiang District'),
(2215, 22, '长寿区', 'Changshou District'),
(2216, 22, '江津区', 'Jiangjin District'),
(2217, 22, '合川区', 'Hechuan District'),
(2218, 22, '永川区', 'Yongchuan District'),
(2219, 22, '南川区', 'Nanchuan District'),
(2220, 22, '璧山区', 'Bishan District'),
(2221, 22, '铜梁区', 'Tongliang District'),
(2222, 22, '潼南区', 'Tongnan District'),
(2223, 22, '荣昌区', 'Rongchang District'),
(2224, 22, '开州区', 'Kai District'),
(2225, 22, '梁平区', 'Liangping District'),
(2226, 22, '武隆区', 'Wulong District'),
(2227, 22, '城口县', 'Chengkou County'),
(2228, 22, '丰都县', 'Fengdu County'),
(2229, 22, '垫江县', 'Dianjiang County'),
(2230, 22, '云阳县', 'Yunyang County'),
(2231, 22, '奉节县', 'Fengjie County'),
(2232, 22, '巫山县', 'Wushan County'),
(2233, 22, '巫溪县', 'Wuxi County'),
(2234, 22, '石柱土家族自治县', 'Shizhu Tujia Autonomous County'),
(2235, 22, '秀山土家族苗族自治县', 'Xiushan Tujia and Miao Autonomous County'),
(2236, 22, '酉阳土家族苗族自治县', 'Youyang Tujia and Miao Autonomous County'),
(2237, 22, '彭水苗族土家族自治县', 'Pengshui Miao and Tujia Autonomous County');

-- Sichuan (23)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(2301, 23, '成都市', 'Chengdu'),
(2302, 23, '自贡市', 'Zigong'),
(2303, 23, '攀枝花市', 'Panzhihua'),
(2304, 23, '泸州市', 'Luzhou'),
(2305, 23, '德阳市', 'Deyang'),
(2306, 23, '绵阳市', 'Mianyang'),
(2307, 23, '广元市', 'Guangyuan'),
(2308, 23, '遂宁市', 'Suining'),
(2309, 23, '内江市', 'Neijiang'),
(2310, 23, '乐山市', 'Leshan'),
(2311, 23, '南充市', 'Nanchong'),
(2312, 23, '眉山市', 'Meishan'),
(2313, 23, '宜宾市', 'Yibin'),
(2314, 23, '广安市', 'Guang''an'),
(2315, 23, '达州市', 'Dazhou'),
(2316, 23, '雅安市', 'Ya''an'),
(2317, 23, '巴中市', 'Bazhong'),
(2318, 23, '资阳市', 'Ziyang'),
(2319, 23, '阿坝藏族羌族自治州', 'Aba Tibetan and Qiang Autonomous Prefecture'),
(2320, 23, '甘孜藏族自治州', 'Garzê Tibetan Autonomous Prefecture'),
(2321, 23, '凉山彝族自治州', 'Liangshan Yi Autonomous Prefecture');

-- Guizhou (24)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(2401, 24, '贵阳市', 'Guiyang'),
(2402, 24, '六盘水市', 'Liupanshui'),
(2403, 24, '遵义市', 'Zunyi'),
(2404, 24, '安顺市', 'Anshun'),
(2405, 24, '毕节市', 'Bijie'),
(2406, 24, '铜仁市', 'Tongren'),
(2407, 24, '黔西南布依族苗族自治州', 'Qianxinan Buyei and Miao Autonomous Prefecture'),
(2408, 24, '黔东南苗族侗族自治州', 'Qiandongnan Miao and Dong Autonomous Prefecture'),
(2409, 24, '黔南布依族苗族自治州', 'Qiannan Buyei and Miao Autonomous Prefecture');

-- Yunnan (25)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(2501, 25, '昆明市', 'Kunming'),
(2502, 25, '曲靖市', 'Qujing'),
(2503, 25, '玉溪市', 'Yuxi'),
(2504, 25, '保山市', 'Baoshan'),
(2505, 25, '昭通市', 'Zhaotong'),
(2506, 25, '丽江市', 'Lijiang'),
(2507, 25, '普洱市', 'Pu''er'),
(2508, 25, '临沧市', 'Lincang'),
(2509, 25, '楚雄彝族自治州', 'Chuxiong Yi Autonomous Prefecture'),
(2510, 25, '红河哈尼族彝族自治州', 'Honghe Hani and Yi Autonomous Prefecture'),
(2511, 25, '文山壮族苗族自治州', 'Wenshan Zhuang and Miao Autonomous Prefecture'),
(2512, 25, '西双版纳傣族自治州', 'Xishuangbanna Dai Autonomous Prefecture'),
(2513, 25, '大理白族自治州', 'Dali Bai Autonomous Prefecture'),
(2514, 25, '德宏傣族景颇族自治州', 'Dehong Dai and Jingpo Autonomous Prefecture'),
(2515, 25, '怒江傈僳族自治州', 'Nujiang Lisu Autonomous Prefecture'),
(2516, 25, '迪庆藏族自治州', 'Diqing Tibetan Autonomous Prefecture');

-- Tibet (26)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(2601, 26, '拉萨市', 'Lhasa'),
(2602, 26, '日喀则市', 'Shigatse'),
(2603, 26, '昌都市', 'Qamdo'),
(2604, 26, '林芝市', 'Nyingchi'),
(2605, 26, '山南市', 'Shannan'),
(2606, 26, '那曲市', 'Nagqu'),
(2607, 26, '阿里地区', 'Ngari Prefecture');

-- Shaanxi (27)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(2701, 27, '西安市', 'Xi''an'),
(2702, 27, '铜川市', 'Tongchuan'),
(2703, 27, '宝鸡市', 'Baoji'),
(2704, 27, '咸阳市', 'Xianyang'),
(2705, 27, '渭南市', 'Weinan'),
(2706, 27, '延安市', 'Yan''an'),
(2707, 27, '汉中市', 'Hanzhong'),
(2708, 27, '榆林市', 'Yulin'),
(2709, 27, '安康市', 'Ankang'),
(2710, 27, '商洛市', 'Shangluo');

-- Gansu (28)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(2801, 28, '兰州市', 'Lanzhou'),
(2802, 28, '嘉峪关市', 'Jiayuguan'),
(2803, 28, '金昌市', 'Jinchang'),
(2804, 28, '白银市', 'Baiyin'),
(2805, 28, '天水市', 'Tianshui'),
(2806, 28, '武威市', 'Wuwei'),
(2807, 28, '张掖市', 'Zhangye'),
(2808, 28, '平凉市', 'Pingliang'),
(2809, 28, '酒泉市', 'Jiuquan'),
(2810, 28, '庆阳市', 'Qingyang'),
(2811, 28, '定西市', 'Dingxi'),
(2812, 28, '陇南市', 'Longnan'),
(2813, 28, '临夏回族自治州', 'Linxia Hui Autonomous Prefecture'),
(2814, 28, '甘南藏族自治州', 'Gannan Tibetan Autonomous Prefecture');

-- Qinghai (29)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(2901, 29, '西宁市', 'Xining'),
(2902, 29, '海东市', 'Haidong'),
(2903, 29, '海北藏族自治州', 'Haibei Tibetan Autonomous Prefecture'),
(2904, 29, '黄南藏族自治州', 'Huangnan Tibetan Autonomous Prefecture'),
(2905, 29, '海南藏族自治州', 'Hainan Tibetan Autonomous Prefecture'),
(2906, 29, '果洛藏族自治州', 'Golog Tibetan Autonomous Prefecture'),
(2907, 29, '玉树藏族自治州', 'Yushu Tibetan Autonomous Prefecture'),
(2908, 29, '海西蒙古族藏族自治州', 'Haixi Mongol and Tibetan Autonomous Prefecture');

-- Ningxia (30)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(3001, 30, '银川市', 'Yinchuan'),
(3002, 30, '石嘴山市', 'Shizuishan'),
(3003, 30, '吴忠市', 'Wuzhong'),
(3004, 30, '固原市', 'Guyuan'),
(3005, 30, '中卫市', 'Zhongwei');

-- Xinjiang (31)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(3101, 31, '乌鲁木齐市', 'Ürümqi'),
(3102, 31, '克拉玛依市', 'Karamay'),
(3103, 31, '吐鲁番市', 'Turpan'),
(3104, 31, '哈密市', 'Hami'),
(3105, 31, '昌吉回族自治州', 'Changji Hui Autonomous Prefecture'),
(3106, 31, '博尔塔拉蒙古自治州', 'Bortala Mongol Autonomous Prefecture'),
(3107, 31, '巴音郭楞蒙古自治州', 'Bayingolin Mongol Autonomous Prefecture'),
(3108, 31, '阿克苏地区', 'Aksu Prefecture'),
(3109, 31, '克孜勒苏柯尔克孜自治州', 'Kizilsu Kirghiz Autonomous Prefecture'),
(3110, 31, '喀什地区', 'Kashgar Prefecture'),
(3111, 31, '和田地区', 'Hotan Prefecture'),
(3112, 31, '伊犁哈萨克自治州', 'Ili Kazakh Autonomous Prefecture'),
(3113, 31, '塔城地区', 'Tacheng Prefecture'),
(3114, 31, '阿勒泰地区', 'Altay Prefecture'),
(3115, 31, '自治区直辖县级行政区划', 'County-level divisions directly under the autonomous region');

-- Taiwan (32)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(3201, 32, '台北市', 'Taipei'),
(3202, 32, '高雄市', 'Kaohsiung'),
(3203, 32, '基隆市', 'Keelung'),
(3204, 32, '台中市', 'Taichung'),
(3205, 32, '台南市', 'Tainan'),
(3206, 32, '新竹市', 'Hsinchu'),
(3207, 32, '嘉义市', 'Chiayi'),
(3208, 32, '新北市', 'New Taipei'),
(3209, 32, '桃园市', 'Taoyuan'),
(3210, 32, '宜兰县', 'Yilan County'),
(3211, 32, '新竹县', 'Hsinchu County'),
(3212, 32, '苗栗县', 'Miaoli County'),
(3213, 32, '彰化县', 'Changhua County'),
(3214, 32, '南投县', 'Nantou County'),
(3215, 32, '云林县', 'Yunlin County'),
(3216, 32, '嘉义县', 'Chiayi County'),
(3217, 32, '屏东县', 'Pingtung County'),
(3218, 32, '台东县', 'Taitung County'),
(3219, 32, '花莲县', 'Hualien County'),
(3220, 32, '澎湖县', 'Penghu County'),
(3221, 32, '金门县', 'Kinmen County'),
(3222, 32, '连江县', 'Lienchiang County');

-- Hong Kong (33)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(3301, 33, '中西区', 'Central and Western District'),
(3302, 33, '东区', 'Eastern District'),
(3303, 33, '南区', 'Southern District'),
(3304, 33, '湾仔区', 'Wan Chai District'),
(3305, 33, '九龙城区', 'Kowloon City District'),
(3306, 33, '观塘区', 'Kwun Tong District'),
(3307, 33, '葵青区', 'Kwai Tsing District'),
(3308, 33, '荃湾区', 'Tsuen Wan District'),
(3309, 33, '屯门区', 'Tuen Mun District'),
(3310, 33, '元朗区', 'Yuen Long District'),
(3311, 33, '北区', 'North District'),
(3312, 33, '大埔区', 'Tai Po District'),
(3313, 33, '西贡区', 'Sai Kung District');

-- Macau (34)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(3401, 34, '花地玛堂区', 'Nossa Senhora de Fátima Parish'),
(3402, 34, '圣安多尼堂区', 'Santo António Parish'),
(3403, 34, '大堂区', 'Sé Parish'),
(3404, 34, '望德堂区', 'São Lázaro Parish'),
(3405, 34, '风顺堂区', 'São Lourenço Parish'),
(3406, 34, '嘉模堂区', 'Our Lady of Carmel Parish'),
(3407, 34, '圣方济各堂区', 'São Francisco Xavier Parish');

-- Abroad (35)
INSERT INTO dbo.tb_Cities (CityID, ProvinceID, CityCNName, CityENName) VALUES
(3501, 35, '其他', 'Other');