class Medication < ActiveRecord::Base
  attr_accessible :bottle_size, :dosage_quant, :dosage_size, :drug, :frequency, :name, :notes, :period, :user_id

  belongs_to :user
  has_many :reminders

end
