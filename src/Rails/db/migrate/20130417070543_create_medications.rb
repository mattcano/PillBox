class CreateMedications < ActiveRecord::Migration
  def change
    create_table :medications do |t|
      t.string :name
      t.string :drug
      t.integer :dosage_quant
      t.string :dosage_size
      t.integer :frequency
      t.string :period
      t.integer :user_id
      t.integer :bottle_size
      t.text :notes

      t.timestamps
    end
  end
end
