import random
import calendar

# 初始化變數
last_saturday_id = None
last_sunday_id = None
previous_id = None  # 前一天的 ID

# 指定年份和月份
year = 2024
month = 6

# 使用 monthrange() 函數獲取第一天的星期索引和該月的天數
first_day_index, num_days = calendar.monthrange(year, month)

# 創建一個空字典來存儲日期和對應的ID
date = {}

# 初始化ID列表和對應名字的字典
doc_id = ["001", "002", "003", "004", "005"]
doc_id_to_name = {
    "001": "林書榆",
    "002": "陳芷芸",
    "003": "萬家妤",
    "004": "曾偉倫",
    "005": "陳麗卿"
}

# 定義每個ID的排除日期和使用限制
restrictions = {
    "001": [0, 5, 10],
    "002": [1, 6, 11],
    "003": [2, 7, 12],
    "004": [3, 8, 13],
    "005": [4, 9, 14]
}
usage_limits = {
    "001": 10,
    "002": 8,
    "003": 6,
    "004": 9,
    "005": 7
}
usage_counts = {id: 0 for id in usage_limits}

# 確保有足夠的ID使用次數來填滿日期陣列
total_usage_limits = sum(usage_limits.values())
if total_usage_limits < num_days:
    raise ValueError(f"指定的ID使用次數總和不足以填滿date陣列。缺少 {num_days - total_usage_limits} 次使用機會。請調整usage_limits。")

# 為每天分配ID
for day in range(1, num_days + 1):
    key = f"{year}-06-{day:02d}"
    weekday = (first_day_index + day - 1) % 7
    
    is_weekend = weekday in [5, 6]  # 星期六或星期天
 
    # 篩選符合條件的ID
    available_ids = [
        id for id in doc_id 
        if day - 1 not in restrictions[id]
        and usage_counts[id] < usage_limits[id]
        and not (is_weekend and id == (last_saturday_id if weekday == 5 else last_sunday_id))
        and id != previous_id  # 確保不與前一天的 ID 相同
    ]

    if available_ids:
        current_id = random.choice(available_ids)
    else:
        raise Exception("No available ID for this day.")

    date[key] = current_id
    usage_counts[current_id] += 1  # 更新使用次數

    if weekday == 5:
        last_saturday_id = current_id
    elif weekday == 6:
        last_sunday_id = current_id

    previous_id = current_id  # 更新前一天的 ID 以供下一天使用

# 輸出date陣列的內容
for k, v in date.items():
    day = int(k.split('-')[-1])
    weekday = (first_day_index + day - 1) % 7
    weekday_tag = ""
    if weekday == 5:
        weekday_tag = " 六"
    elif weekday == 6:
        weekday_tag = " 日"

    print(f"{k}: {v}, 名字: {doc_id_to_name[v]}{weekday_tag}")