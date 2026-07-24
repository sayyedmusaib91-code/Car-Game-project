import sqlite3
from datetime import datetime
import pytz

def init_db():
    conn = sqlite3.connect('database.db')
    cursor = conn.cursor()

    # USERS TABLE
    cursor.execute('''
        CREATE TABLE IF NOT EXISTS users (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            username TEXT UNIQUE NOT NULL,
            password TEXT NOT NULL,
            player_id TEXT NOT NULL,
            level INTEGER DEFAULT 1,
            total_score INTEGER DEFAULT 0
        )
    ''')

    # RACE RESULTS TABLE
    cursor.execute('''
        CREATE TABLE IF NOT EXISTS race_results (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            user_id INTEGER,
            position INTEGER,
            score INTEGER,
            race_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            FOREIGN KEY(user_id) REFERENCES users(id)
        )
    ''')

    conn.commit()
    conn.close()


# ADD USER
def add_user(username, password, player_id):
    try:
        conn = sqlite3.connect('database.db')
        cursor = conn.cursor()

        cursor.execute(
            'INSERT INTO users (username, password, player_id) VALUES (?, ?, ?)',
            (username, password, player_id)
        )

        conn.commit()
        conn.close()
        return True

    except sqlite3.IntegrityError:
        return False


# CHECK LOGIN
def check_user(username, password):
    conn = sqlite3.connect('database.db')
    cursor = conn.cursor()

    cursor.execute(
        'SELECT * FROM users WHERE username = ? AND password = ?',
        (username, password)
    )

    user = cursor.fetchone()
    conn.close()

    return user


# SAVE RACE RESULT
def save_race_result(user_id, position, score):
    conn = sqlite3.connect('database.db')
    cursor = conn.cursor()

    # Indian time
    ist = pytz.timezone('Asia/Kolkata')
    race_time = datetime.now(ist).strftime("%Y-%m-%d %H:%M:%S")

    # Insert race result
    cursor.execute(
        'INSERT INTO race_results (user_id, position, score, race_date) VALUES (?, ?, ?, ?)',
        (user_id, position, score, race_time)
    )

    # Update total score
    cursor.execute(
        'UPDATE users SET total_score = total_score + ? WHERE id = ?',
        (score, user_id)
    )

    # Auto level update
    cursor.execute(
        'UPDATE users SET level = CAST(total_score / 1000 AS INTEGER) + 1 WHERE id = ?',
        (user_id,)
    )

    conn.commit()
    conn.close()


# GET PROFILE DATA
def get_profile_data(user_id):
    conn = sqlite3.connect('database.db')
    cursor = conn.cursor()

    # Get user info
    cursor.execute(
        'SELECT username, player_id, level, total_score FROM users WHERE id = ?',
        (user_id,)
    )
    user = cursor.fetchone()

    # Get last 10 races
    cursor.execute(
        '''
        SELECT position, score, race_date
        FROM race_results
        WHERE user_id = ?
        ORDER BY race_date DESC
        LIMIT 10
        ''',
        (user_id,)
    )
    races = cursor.fetchall()

    conn.close()

    # ✅ Convert race_date to 12-hour format
    formatted_races = []
    for pos, score, race_date in races:
        dt_obj = datetime.strptime(race_date, "%Y-%m-%d %H:%M:%S")
        formatted_date = dt_obj.strftime("%d-%m-%Y %I:%M %p")  # 12-hour
        formatted_races.append((pos, score, formatted_date))

    return user, formatted_races
