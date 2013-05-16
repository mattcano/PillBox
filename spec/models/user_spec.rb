require_relative '../spec_helper'

describe User do
  

  before do 
    @user = User.create!(
      :name => "Matt",
      :email => "mail@gmail.com",
      :password => "password123",
      :password_confirmation => "password123"
    )
  end

  after do
    User.destroy_all
  end
  
  it "has a name" do
    expect(@user.name).to eq "Matt"
  end

  it "can have multiple medications" do
    med1 = Medication.create(:user_id => @user.id)
    med2 = Medication.create(:user_id => @user.id)
    expect(@user.medications).to eq [med1, med2]
  end

end