class PillboxController < ApplicationController

  before_filter :authenticate_user! 
  before_filter :update_meds_array, :only => [:mypillbox, :meds_list]
  before_filter :update_reminder_array, :only => [:mypillbox, :reminders]

  def mypillbox
  end

  def reminders
  end

  def meds_list
  end

  def coaches
  end

  def buddies
  end

  def settings
  end

private

  def update_meds_array
    @meds_array = current_user.medications.order(:name)
  end

  def update_reminder_array
    @reminder_array = current_user.reminders.order(:date)
  end

end
