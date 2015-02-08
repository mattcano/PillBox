class Reminder < ActiveRecord::Base
  attr_accessible :date, :medication_id, :message, :user_id

  belongs_to :user
  belongs_to :medication

end
