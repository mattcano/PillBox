require_relative '../spec_helper'

describe User do
  
  it "has a name" do
    user = User.new(:name => "Matt")
    expect(user.name).to eq "Matt"
  end

end