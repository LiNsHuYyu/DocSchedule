from flask import Flask, render_template
import calendar_manager

app = Flask(__name__)

@app.route('/')
def index():
    # 從 calendar_manager 模組獲取 date 陣列
    date = calendar_manager.date
    # 傳遞 doc_id 到名字的對應字典到模板
    return render_template('index.html', date=date, doc_id_to_name=calendar_manager.doc_id_to_name)

if __name__ == '__main__':
    app.run(debug=True)