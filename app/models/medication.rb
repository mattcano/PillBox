class Medication < ActiveRecord::Base
  attr_accessible :bottle_size, :dosage_quant, :dosage_size, :drug, :frequency,
                  :name, :notes, :period, :user_id

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
    now = Time.now
    if self.period == "per day"
      while num > 0 
        if self.frequency == 1
          time = 9
          now = now.tomorrow unless now.hour < time
          Reminder.create(
            date: Time.new(now.year, now.month, now.day, time).localtime, 
            user_id: self.user_id, 
            medication_id: self.id,
            message: "Take #{self.dosage_quant} #{self.dosage_size} of #{self.name} at #{time}:00#{time<12 ? "AM" : "PM"}."
            )
          num -= 1
        end
      end
    end
  end

protected
  


end
