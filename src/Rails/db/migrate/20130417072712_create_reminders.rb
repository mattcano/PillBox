class CreateReminders < ActiveRecord::Migration
  def change
    create_table :reminders do |t|
      t.datetime :date
      t.integer :user_id
      t.integer :medication_id
      t.text :message

      t.timestamps
    end
  end
end
