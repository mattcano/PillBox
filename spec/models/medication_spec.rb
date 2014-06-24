require_relative '../spec_helper'
describe "Medication Model" do

	describe "Medication create" do

		before do
			@medication = Medication.create!(
				:bottle_size => 200,
				:dosage_quant => 2,
				:dosage_size => "pills",
				:drug => "hydrocodone",
				:frequency => 1,
				:name => "Vicotin",
				:notes => "Some notes",
				:added_by => 1
				)
		end

		after do
			#Medication.destory_all
		end

		it "should have the field added_by" do
			expect(@medication.added_by).to eq 1
		end


	end

# describe Medication do 

#   after do
#     Medication.destroy_all
#     Reminder.destroy_all
#   end

#   describe "create_reminders" do 

#     it "creates n new reminders" do
#       med = Medication.create!(user_id: 4, name: "Alpha", dosage_quant: 1, dosage_size: "pills")
#     end

#   end

end