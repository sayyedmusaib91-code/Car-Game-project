from flask import Flask, render_template, request, redirect, url_for, flash, session, jsonify
import database
import random 
from database import save_race_result
from flask_cors import CORS 


                                                                               
app = Flask(__name__)
CORS(app)
app.secret_key = "racer_secret_key" # Bina iske flash kaam nahi karega

database.init_db()

def generate_player_id():
    return str(random.randint(1000000000,9999999999))

@app.route('/')
def home():
    return render_template('index.html')

@app.route('/intro')
def intro():
    return render_template('intro.html')

# 1. Sabse pehle home page ko login page par redirect karo


# 2. Login Page Function (Iska naam 'login' rakho)
@app.route('/login', methods=['GET', 'POST'])
def login():
    if request.method == 'POST':
        user_in = request.form.get('username')
        pass_in = request.form.get('password')
        
        user = database.check_user(user_in, pass_in)
        if user:
            session['username'] = user_in
            session['player_id'] = user[3]
            session['user_id'] = user[0]
            return redirect(url_for('loading'))
        else:
            flash("Invalid Login! Please enter correct details.")
            return redirect(url_for('login',fromLogout='true')) # Ab error nahi aayega
            
    return render_template('login.html')

# 3. Signup Page Function
@app.route('/signup', methods=['GET', 'POST'])
def signup():
    if request.method == 'POST':
        user_in = request.form.get('username')
        pass_in = request.form.get('password')

        player_id = generate_player_id()
        
        if database.add_user(user_in, pass_in, player_id):
            flash("Account created succefully! Please Login.")
            return redirect(url_for('login')) # Yahan bhi 'login' use karein
        else:
            flash("Username already exists!")
            
    return render_template('signup.html')

@app.route('/loading')
def loading():
    return render_template('loading.html')
    

@app.route('/login_guest')
def login_guest():
    session.clear()
    session['username'] = 'Guest'
    session['user_id'] = -1
    session['player_id'] = 'GUEST'
    return redirect(url_for('loading'))

@app.route('/main_menu')
def main_menu():
    username = session.get('username','Guest')
    player_id = session.get('player_id','0000000000')
    return render_template('main_menu.html',username=username,player_id=player_id)

@app.route('/logout')
def logout():
    session.clear() # Ye command session se user ka data hata degi
    flash("You have been logged out.",)
    return redirect(url_for('login',fromSignup='true')) # Wapas login page par bhej degi

@app.route('/save_race_result', methods=['POST'])
def save_race():

    data = request.get_json()

    user_id = session.get('user_id')   # ⭐ session se lo
    position = data.get("position")
    score = data.get("score")

    print("POSITION RECEIVED:", position)
    print("SCORE RECEIVED:", score)

    if not user_id:
        return jsonify({"error": "Not logged in"}), 401

    save_race_result(user_id, position, score)

    return jsonify({"status": "success"})

@app.route('/get_profile')
def get_profile():

    if "user_id" not in session:
        return jsonify({"error": "Not logged in"}), 401

    user_id = session["user_id"]

    # ⭐ Guest user
    if user_id == -1:
        return jsonify({
            "username": "Guest",
            "player_id": "GUEST",
            "level": 1,
            "total_score": 0,
            "races": []
        })

    user, races = database.get_profile_data(user_id)

    if user is None:
        return jsonify({"error": "User not found"}), 401

    return jsonify({
        "username": user[0],
        "player_id": user[1],
        "level": user[2],
        "total_score": user[3],
        "races": [
            {
                "position": r[0],
                "score": r[1],
                "race_date": r[2]
            }
            for r in races
        ]
    })

@app.route("/check")
def check():
    import sqlite3
    conn = sqlite3.connect("database.db")
    cursor = conn.cursor()
    cursor.execute("PRAGMA table_info(users)")
    return str(cursor.fetchall())

@app.route('/privacy_policy')
def privacy_policy():
    return render_template('privacy_policy.html')

@app.route('/shop')
def shop():
    return render_template('shop.html')

@app.route('/profile')
def profile():

    user_id = session.get('user_id')

    if user_id is None:
        return redirect(url_for('login'))


     # Guest profile
    if user_id == -1:
        return render_template(
            "profile.html",
            username="Guest",
            player_id="GUEST",
            level=1,
            total_score=0,
            current_xp=0,
            max_xp=1000,
            races=[],
            level_up=False   # 🔥 ADD THIS
        )

    user, races = database.get_profile_data(user_id)


    total_score = user[3]

    # 🔥 Level system
    level = total_score // 1000 + 1
    current_xp = total_score % 1000
    max_xp = 1000

        # 🔥 NEW LOGIC
    old_level = session.get('last_level', 1)

    level_up = False
    if level > old_level:
        level_up = True

    session['last_level'] = level

    print("LEVEL UP VALUE:", level_up)
    

    

    return render_template(
        "profile.html",
        username=user[0],
        player_id=user[1],
        level=level,
        total_score=total_score,
        current_xp=current_xp,
        max_xp=max_xp,
        races=races,
        level_up=level_up   # 🔥 ADD THIS
    )



if __name__ == '__main__':
    app.run(debug=True)