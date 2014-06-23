class Medication < ActiveRecord::Base
  attr_accessible :bottle_size, :dosage_quant, :dosage_size, :drug, :frequency,
                  :name, :notes, :period, :user_id, :added_by

  belongs_to :user
  has_many :reminders

  # 2 times for reminders 9am and 6pm
  @morning = 9
  @evening = 18

  # TBC
  # returns the time of the next reminder after a given time
  def next_reminder(time)
    # return next_time
  end

  # Creates the next "num" reminders
  def create_reminders(num)
    reminders_created = false
    now = Time.now
    # if self.period == "per day"
      # if self.frequency == 1
        while num > 0
          time = 9
          now = now.tomorrow unless now.hour < time
          Reminder.create(
            date: Time.new(now.year, now.month, now.day, time).localtime, 
            user_id: self.user_id, 
            medication_id: self.id,
            message: "take #{self.dosage_quant} #{self.dosage_size} of #{self.name} at #{time}:00#{time<12 ? "AM" : "PM"}"
          )
          num -= 1
        end
        reminders_created = true
      # end
    # end
    remove_old_reminders if reminders_created
  end

private

  def remove_old_reminders
    Reminder.where("user_id = ? AND medication_id = ? AND created_at < ?", self.user_id, self.id, (Time.now - 5)).destroy_all
  end
  
end
