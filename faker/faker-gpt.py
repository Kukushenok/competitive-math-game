# I'm astonished but this is just DeepSeek response to my "request".. :P
# IT IS WORKING PERFECTLY!!
import psycopg2
from faker import Faker
import random
from datetime import datetime, timedelta

def generate_data(num_accounts=10, num_rewards=5, num_competitions=3, num_participations=20, num_competition_rewards=8, num_player_rewards=15):
    # Connect to PostgreSQL
    conn = psycopg2.connect(
        dbname="tester",
        user="tester",
        password="tester",
        host="127.0.0.1",
        port="53253"
    )
    cursor = conn.cursor()
    
    fake = Faker()
    
    try:
        # Generate accounts
        print(f"Generating {num_accounts} accounts...")
        for _ in range(num_accounts):
            login = fake.user_name()[:32]
            while ' ' in login:
                login = fake.user_name()[:32]
            
            cursor.execute(
                "INSERT INTO account (login, email, password_hash, privilegy_level, description, profile_image) "
                "VALUES (%s, %s, %s, %s, %s, NULL)",
                (
                    login,
                    fake.email()[:32],
                    fake.sha256()[:64],
                    random.randint(1, 10),
                    fake.text(max_nb_chars=128)[:128]
                )
            )
        
        # Get account IDs for foreign keys
        cursor.execute("SELECT id FROM account")
        account_ids = [row[0] for row in cursor.fetchall()]
        
        # Generate reward descriptions
        print(f"Generating {num_rewards} reward descriptions...")
        for _ in range(num_rewards):
            cursor.execute(
                "INSERT INTO reward_description (reward_name, description, icon_image, ingame_data) "
                "VALUES (%s, %s, NULL, NULL)",
                (
                    fake.catch_phrase()[:64],
                    fake.text(max_nb_chars=128)[:128]
                )
            )
        
        # Get reward description IDs
        cursor.execute("SELECT id FROM reward_description")
        reward_description_ids = [row[0] for row in cursor.fetchall()]
        
        # Generate competitions
        print(f"Generating {num_competitions} competitions...")
        competition_ids = []
        for _ in range(num_competitions):
            start_time = fake.date_time_this_year()
            end_time = start_time + timedelta(days=random.randint(1, 30))
            has_ended = end_time < datetime.now()
            
            cursor.execute(
                "INSERT INTO competition (competition_name, description, start_time, end_time, level_data, has_ended) "
                "VALUES (%s, %s, %s, %s, NULL, %s) RETURNING id",
                (
                    fake.catch_phrase()[:64],
                    fake.text(max_nb_chars=128)[:128],
                    start_time,
                    end_time,
                    has_ended
                )
            )
            competition_ids.append(cursor.fetchone()[0])
        
        # Generate player participations
        print(f"Generating {num_participations} player participations...")
        for _ in range(num_participations):
            cursor.execute(
                "INSERT INTO player_participation (competition_id, account_id, score) "
                "VALUES (%s, %s, %s) "
                "ON CONFLICT (competition_id, account_id) DO NOTHING",
                (
                    random.choice(competition_ids),
                    random.choice(account_ids),
                    random.randint(0, 10000)
                )
            )
        
        # Generate competition rewards
        print(f"Generating {num_competition_rewards} competition rewards...")
        for _ in range(num_competition_rewards):
            cursor.execute(
                "INSERT INTO competition_reward (reward_description_id, competition_id, condition) "
                "VALUES (%s, %s, %s)",
                (
                    random.choice(reward_description_ids),
                    random.choice(competition_ids),
                    '{"min_score": ' + str(random.randint(100, 5000)) + '}'
                )
            )
        
        # Generate player rewards
        print(f"Generating {num_player_rewards} player rewards...")
        for _ in range(num_player_rewards):
            cursor.execute(
                "INSERT INTO player_reward (reward_description_id, player_id, competition_id) "
                "VALUES (%s, %s, %s)",
                (
                    random.choice(reward_description_ids),
                    random.choice(account_ids),
                    random.choice(competition_ids) if random.random() > 0.3 else None
                )
            )
        
        conn.commit()
        print("Data generation completed successfully!")
    
    except Exception as e:
        conn.rollback()
        print(f"Error: {e}")
    finally:
        cursor.close()
        conn.close()

if __name__ == "__main__":
    # Customize the number of records to generate for each table
    generate_data(
        num_accounts=200,
        num_rewards=20,
        num_competitions=5,
        num_participations=1000,
        num_competition_rewards=20,
        num_player_rewards=1
    )