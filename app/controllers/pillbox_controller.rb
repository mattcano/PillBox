class PillboxController < ApplicationController

  before_filter :authenticate_user! 
  before_filter :update_meds_array

  def update_meds_array
    @meds_array = current_user.medications.order(:name)
    @counter = 1
  end

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

end
